using System.Net.Sockets;
using FluentAssertions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MedReminder.Application.Abstractions;
using MedReminder.Infrastructure.Email;
using Xunit;

namespace MedReminder.Infrastructure.Tests.Email;

// Android backlog B5-01: the connection test of a configuration not saved
// yet, in categories a form turns into recovery guidance. Runs against a
// scripted loopback server; nothing is sent.
public class SmtpConnectionTesterTests
{
    private readonly SmtpConnectionTester _tester = new();

    private static SmtpTransport Transport(int port, string username = "", bool startTls = true, int timeout = 5)
        => new("127.0.0.1", port, startTls, username, "home@example.org", "MedReminder", timeout);

    [Fact]
    public async Task A_server_without_sign_in_is_accepted_and_reported_as_not_encrypted()
    {
        await using var server = new FakeSmtpServer();

        var result = await _tester.TestAsync(Transport(server.Port), password: null, CancellationToken.None);

        result.Outcome.Should().Be(SmtpTestOutcome.Accepted);
        result.Encrypted.Should().BeFalse();
        server.AuthenticationAttempts.Should().Be(0);
    }

    [Fact]
    public async Task The_right_credentials_are_accepted()
    {
        await using var server = new FakeSmtpServer(user: "home", password: "s3cret");

        var result = await _tester.TestAsync(Transport(server.Port, "home"), "s3cret", CancellationToken.None);

        result.Outcome.Should().Be(SmtpTestOutcome.Accepted);
        server.AuthenticationAttempts.Should().Be(1);
    }

    [Fact]
    public async Task A_wrong_password_is_an_authentication_failure()
    {
        await using var server = new FakeSmtpServer(user: "home", password: "s3cret");

        var result = await _tester.TestAsync(Transport(server.Port, "home"), "wrong", CancellationToken.None);

        result.Outcome.Should().Be(SmtpTestOutcome.AuthenticationFailed);
    }

    [Fact]
    public async Task A_user_name_on_a_server_without_sign_in_is_reported()
    {
        await using var server = new FakeSmtpServer(offerAuth: false);

        var result = await _tester.TestAsync(Transport(server.Port, "home"), "s3cret", CancellationToken.None);

        result.Outcome.Should().Be(SmtpTestOutcome.AuthenticationNotOffered);
    }

    [Fact]
    public async Task A_closed_port_is_a_connection_failure()
    {
        var result = await _tester.TestAsync(Transport(FakeSmtpServer.ClosedPort()), null, CancellationToken.None);

        result.Outcome.Should().Be(SmtpTestOutcome.ConnectionFailed);
    }

    [Fact]
    public async Task A_server_that_never_greets_times_out()
    {
        await using var server = new FakeSmtpServer(FakeSmtpServer.Behaviour.Silent);

        var result = await _tester.TestAsync(Transport(server.Port, timeout: 1), null, CancellationToken.None);

        result.Outcome.Should().Be(SmtpTestOutcome.TimedOut);
    }

    [Fact]
    public async Task A_refused_service_is_rejected()
    {
        await using var server = new FakeSmtpServer(FakeSmtpServer.Behaviour.RefuseService);

        var result = await _tester.TestAsync(Transport(server.Port), null, CancellationToken.None);

        result.Outcome.Should().Be(SmtpTestOutcome.Rejected);
    }

    [Fact]
    public async Task A_failed_TLS_handshake_is_a_secure_connection_failure()
    {
        await using var server = new FakeSmtpServer(FakeSmtpServer.Behaviour.BrokenTls);

        var result = await _tester.TestAsync(Transport(server.Port), null, CancellationToken.None);

        result.Outcome.Should().Be(SmtpTestOutcome.SecureConnectionFailed);
        result.Encrypted.Should().BeFalse();
    }

    [Fact]
    public async Task An_incomplete_configuration_or_a_missing_password_tries_nothing()
    {
        await using var server = new FakeSmtpServer();

        (await _tester.TestAsync(Transport(server.Port) with { Host = " " }, null, CancellationToken.None))
            .Outcome.Should().Be(SmtpTestOutcome.NotConfigured);
        (await _tester.TestAsync(Transport(server.Port) with { FromAddress = "" }, null, CancellationToken.None))
            .Outcome.Should().Be(SmtpTestOutcome.NotConfigured);
        (await _tester.TestAsync(Transport(server.Port, "home"), "", CancellationToken.None))
            .Outcome.Should().Be(SmtpTestOutcome.PasswordMissing);
        server.AuthenticationAttempts.Should().Be(0);
    }

    [Fact]
    public async Task The_callers_cancellation_is_not_reported_as_a_timeout()
    {
        await using var server = new FakeSmtpServer(FakeSmtpServer.Behaviour.Silent);
        using var cancel = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));

        await FluentActions.Awaiting(() => _tester.TestAsync(Transport(server.Port, timeout: 30), null, cancel.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }

    [Theory]
    [InlineData(SocketError.HostNotFound, SmtpTestOutcome.HostNotFound)]
    [InlineData(SocketError.TryAgain, SmtpTestOutcome.HostNotFound)]
    [InlineData(SocketError.ConnectionRefused, SmtpTestOutcome.ConnectionFailed)]
    [InlineData(SocketError.NetworkUnreachable, SmtpTestOutcome.ConnectionFailed)]
    [InlineData(SocketError.TimedOut, SmtpTestOutcome.TimedOut)]
    public void Socket_errors_are_classified(SocketError error, SmtpTestOutcome expected)
    {
        SmtpConnectionTester.Classify(new SocketException((int)error)).Should().Be(expected);
        SmtpConnectionTester.Classify(new IOException("wrapped", new SocketException((int)error))).Should().Be(expected);
    }

    [Fact]
    public void Other_errors_are_classified()
    {
        SmtpConnectionTester.Classify(new AuthenticationException("535")).Should().Be(SmtpTestOutcome.AuthenticationFailed);
        SmtpConnectionTester.Classify(new System.Security.Authentication.AuthenticationException("tls"))
            .Should().Be(SmtpTestOutcome.SecureConnectionFailed);
        SmtpConnectionTester.Classify(new TimeoutException()).Should().Be(SmtpTestOutcome.TimedOut);
        SmtpConnectionTester.Classify(new SmtpProtocolException("bad")).Should().Be(SmtpTestOutcome.Rejected);
        SmtpConnectionTester.Classify(new NotSupportedException()).Should().Be(SmtpTestOutcome.AuthenticationNotOffered);
    }

    [Fact]
    public void The_security_option_is_the_senders()
    {
        SmtpConnectionTester.SecureOption(useStartTls: true).Should().Be(SecureSocketOptions.StartTlsWhenAvailable);
        SmtpConnectionTester.SecureOption(useStartTls: false).Should().Be(SecureSocketOptions.Auto);
    }
}
