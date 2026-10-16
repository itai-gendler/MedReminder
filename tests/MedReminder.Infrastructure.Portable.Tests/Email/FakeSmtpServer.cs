using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MedReminder.Infrastructure.Tests.Email;

// A scripted SMTP server on the loopback interface for the connection
// tests (B5-01): one client at a time, only what the tester uses
// (greeting, EHLO, STARTTLS with a real TLS upgrade on a self-signed
// certificate, AUTH PLAIN, QUIT). Nothing is relayed.
internal sealed class FakeSmtpServer : IAsyncDisposable
{
    public enum Behaviour
    {
        // Greets and answers.
        Normal,
        // Accepts the connection and never greets.
        Silent,
        // Greets with 554 (service not available).
        RefuseService,
        // Offers STARTTLS and answers the handshake with plain text.
        BrokenTls,
    }

    private static readonly Lazy<X509Certificate2> Certificate = new(CreateCertificate);

    private readonly TcpListener _listener = new(IPAddress.Loopback, 0);
    private readonly CancellationTokenSource _stop = new();
    private readonly Task _loop;
    private readonly Behaviour _behaviour;
    private readonly bool _offerAuth;
    private readonly bool _offerStartTls;
    private readonly string? _user;
    private readonly string? _password;

    public FakeSmtpServer(Behaviour behaviour = Behaviour.Normal, bool offerAuth = true, bool offerStartTls = false,
        string? user = null, string? password = null)
    {
        _behaviour = behaviour;
        _offerAuth = offerAuth;
        _offerStartTls = offerStartTls;
        _user = user;
        _password = password;
        _listener.Start();
        _loop = Task.Run(AcceptAsync);
    }

    public int Port => ((IPEndPoint)_listener.LocalEndpoint).Port;

    public int AuthenticationAttempts { get; private set; }

    // Whether the last session upgraded to TLS before it ended.
    public bool Upgraded { get; private set; }

    public async ValueTask DisposeAsync()
    {
        await _stop.CancelAsync();
        _listener.Stop();
        try { await _loop; } catch (Exception) { }
        _stop.Dispose();
    }

    // A loopback port nothing listens on.
    public static int ClosedPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }

    private async Task AcceptAsync()
    {
        while (!_stop.IsCancellationRequested)
        {
            TcpClient client;
            try
            {
                client = await _listener.AcceptTcpClientAsync(_stop.Token);
            }
            catch (Exception)
            {
                return;
            }
            using (client)
            {
                try
                {
                    await ServeAsync(client.GetStream(), _stop.Token);
                }
                catch (Exception)
                {
                    // The client went away.
                }
            }
        }
    }

    private async Task ServeAsync(Stream network, CancellationToken ct)
    {
        Stream stream = network;
        var tls = false;
        var (reader, writer) = Open(stream);

        switch (_behaviour)
        {
            case Behaviour.Silent:
                await Task.Delay(Timeout.Infinite, ct);
                return;
            case Behaviour.RefuseService:
                await writer.WriteLineAsync("554 localhost service not available");
                await reader.ReadLineAsync(ct);
                await writer.WriteLineAsync("221 bye");
                return;
        }

        await writer.WriteLineAsync("220 localhost fake SMTP ready");
        try
        {
            while (await reader.ReadLineAsync(ct) is { } line)
            {
                var command = line.ToUpperInvariant();
                if (command.StartsWith("EHLO", StringComparison.Ordinal))
                {
                    await writer.WriteLineAsync("250-localhost");
                    if (!tls && (_offerStartTls || _behaviour == Behaviour.BrokenTls)) await writer.WriteLineAsync("250-STARTTLS");
                    if (_offerAuth) await writer.WriteLineAsync("250-AUTH PLAIN");
                    await writer.WriteLineAsync("250 8BITMIME");
                }
                else if (command.StartsWith("HELO", StringComparison.Ordinal))
                {
                    await writer.WriteLineAsync("250 localhost");
                }
                else if (command == "STARTTLS")
                {
                    await writer.WriteLineAsync("220 go ahead");
                    if (_behaviour == Behaviour.BrokenTls)
                    {
                        // Not a TLS record: the handshake fails on the client.
                        await writer.WriteLineAsync("this is not TLS");
                        return;
                    }
                    reader.Dispose();
                    await writer.DisposeAsync();
                    var secure = new SslStream(network, leaveInnerStreamOpen: true);
                    await secure.AuthenticateAsServerAsync(Certificate.Value);
                    stream = secure;
                    tls = true;
                    Upgraded = true;
                    (reader, writer) = Open(stream);
                }
                else if (command.StartsWith("AUTH PLAIN", StringComparison.Ordinal))
                {
                    AuthenticationAttempts++;
                    var initial = line.Length > "AUTH PLAIN".Length ? line["AUTH PLAIN".Length..].Trim() : null;
                    if (string.IsNullOrEmpty(initial))
                    {
                        await writer.WriteLineAsync("334 ");
                        initial = await reader.ReadLineAsync(ct);
                    }
                    await writer.WriteLineAsync(Accepts(initial)
                        ? "235 2.7.0 Authentication successful"
                        : "535 5.7.8 Authentication credentials invalid");
                }
                else if (command == "QUIT")
                {
                    await writer.WriteLineAsync("221 bye");
                    return;
                }
                else
                {
                    await writer.WriteLineAsync("502 command not implemented");
                }
            }
        }
        finally
        {
            reader.Dispose();
            await writer.DisposeAsync();
            if (stream != network) await stream.DisposeAsync();
        }
    }

    private static (StreamReader Reader, StreamWriter Writer) Open(Stream stream)
        => (new StreamReader(stream, Encoding.ASCII, detectEncodingFromByteOrderMarks: false, leaveOpen: true),
            new StreamWriter(stream, Encoding.ASCII, leaveOpen: true) { NewLine = "\r\n", AutoFlush = true });

    // AUTH PLAIN: base64 of "authzid\0user\0password".
    private bool Accepts(string? initial)
    {
        if (string.IsNullOrEmpty(initial)) return false;
        string decoded;
        try
        {
            decoded = Encoding.UTF8.GetString(Convert.FromBase64String(initial));
        }
        catch (FormatException)
        {
            return false;
        }
        var parts = decoded.Split('\0');
        return parts.Length == 3 && parts[1] == _user && parts[2] == _password;
    }

    // A self-signed certificate for 127.0.0.1, with its private key usable
    // by SslStream on every OS.
    private static X509Certificate2 CreateCertificate()
    {
        using var key = RSA.Create(2048);
        var request = new CertificateRequest("CN=127.0.0.1", key, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var names = new SubjectAlternativeNameBuilder();
        names.AddIpAddress(IPAddress.Loopback);
        request.CertificateExtensions.Add(names.Build());
        using var created = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(30));
        return X509CertificateLoader.LoadPkcs12(created.Export(X509ContentType.Pfx), password: null);
    }
}
