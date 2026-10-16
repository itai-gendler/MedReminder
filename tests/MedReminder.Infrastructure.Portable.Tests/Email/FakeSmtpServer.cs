using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MedReminder.Infrastructure.Tests.Email;

// A scripted SMTP server on the loopback interface for the connection
// tests (B5-01): one client at a time, plain text, only what the tester
// uses (greeting, EHLO, STARTTLS, AUTH PLAIN, QUIT). Nothing is relayed.
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

    private readonly TcpListener _listener = new(IPAddress.Loopback, 0);
    private readonly CancellationTokenSource _stop = new();
    private readonly Task _loop;
    private readonly Behaviour _behaviour;
    private readonly bool _offerAuth;
    private readonly string? _user;
    private readonly string? _password;

    public FakeSmtpServer(Behaviour behaviour = Behaviour.Normal, bool offerAuth = true,
        string? user = null, string? password = null)
    {
        _behaviour = behaviour;
        _offerAuth = offerAuth;
        _user = user;
        _password = password;
        _listener.Start();
        _loop = Task.Run(AcceptAsync);
    }

    public int Port => ((IPEndPoint)_listener.LocalEndpoint).Port;

    public int AuthenticationAttempts { get; private set; }

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

    private async Task ServeAsync(NetworkStream stream, CancellationToken ct)
    {
        using var reader = new StreamReader(stream, Encoding.ASCII, leaveOpen: true);
        await using var writer = new StreamWriter(stream, Encoding.ASCII, leaveOpen: true) { NewLine = "\r\n", AutoFlush = true };

        switch (_behaviour)
        {
            case Behaviour.Silent:
                await Task.Delay(Timeout.Infinite, ct);
                return;
            case Behaviour.RefuseService:
                await writer.WriteLineAsync("554 localhost service not available");
                await ReadAsync(reader, ct);
                await writer.WriteLineAsync("221 bye");
                return;
        }

        await writer.WriteLineAsync("220 localhost fake SMTP ready");
        while (await ReadAsync(reader, ct) is { } line)
        {
            var command = line.ToUpperInvariant();
            if (command.StartsWith("EHLO", StringComparison.Ordinal))
            {
                await writer.WriteLineAsync("250-localhost");
                if (_behaviour == Behaviour.BrokenTls) await writer.WriteLineAsync("250-STARTTLS");
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
                // Not a TLS record: the handshake fails on the client.
                await writer.WriteLineAsync("this is not TLS");
                return;
            }
            else if (command.StartsWith("AUTH PLAIN", StringComparison.Ordinal))
            {
                AuthenticationAttempts++;
                var initial = line.Length > "AUTH PLAIN".Length ? line["AUTH PLAIN".Length..].Trim() : null;
                if (string.IsNullOrEmpty(initial))
                {
                    await writer.WriteLineAsync("334 ");
                    initial = await ReadAsync(reader, ct);
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

    private static async Task<string?> ReadAsync(StreamReader reader, CancellationToken ct)
        => await reader.ReadLineAsync(ct);
}
