using FluentAssertions;
using MedReminder.Application.Abstractions;
using MedReminder.Infrastructure.Email;
using Xunit;

namespace MedReminder.Infrastructure.Tests.Email;

// Android backlog B5-01: the checks of an SMTP form, the rules the MailKit
// sender relies on.
public class SmtpTransportRulesTests
{
    private static readonly SmtpTransport Valid =
        new("smtp.example.org", 587, true, "home", "home@example.org", "MedReminder", 30);

    [Fact]
    public void A_complete_configuration_has_no_problem()
    {
        SmtpTransportRules.Check(Valid, passwordAvailable: true).Should().BeEmpty();
        SmtpTransportRules.Check(Valid with { Username = "" }, passwordAvailable: false).Should().BeEmpty();
        SmtpTransportRules.Check(Valid with { Host = "192.168.1.10", Port = 465 }, true).Should().BeEmpty();
    }

    [Theory]
    [InlineData("", SmtpTransportProblem.HostMissing)]
    [InlineData("   ", SmtpTransportProblem.HostMissing)]
    [InlineData("smtp://smtp.example.org", SmtpTransportProblem.HostInvalid)]
    [InlineData("smtp example.org", SmtpTransportProblem.HostInvalid)]
    public void The_host_is_a_server_name_or_address(string host, SmtpTransportProblem expected)
        => SmtpTransportRules.Check(Valid with { Host = host }, true).Should().Equal(expected);

    [Theory]
    [InlineData(0)]
    [InlineData(65536)]
    public void The_port_is_in_range(int port)
        => SmtpTransportRules.Check(Valid with { Port = port }, true).Should().Equal(SmtpTransportProblem.PortOutOfRange);

    [Theory]
    [InlineData(4)]
    [InlineData(301)]
    public void The_timeout_is_in_range(int seconds)
        => SmtpTransportRules.Check(Valid with { TimeoutSeconds = seconds }, true)
            .Should().Equal(SmtpTransportProblem.TimeoutOutOfRange);

    [Theory]
    [InlineData("", SmtpTransportProblem.FromMissing)]
    [InlineData("not-an-email", SmtpTransportProblem.FromInvalid)]
    [InlineData("home@", SmtpTransportProblem.FromInvalid)]
    [InlineData("a@example.org, b@example.org", SmtpTransportProblem.FromInvalid)]
    public void The_sender_is_one_full_mailbox(string from, SmtpTransportProblem expected)
        => SmtpTransportRules.Check(Valid with { FromAddress = from }, true).Should().Equal(expected);

    [Fact]
    public void A_user_name_needs_a_password()
        => SmtpTransportRules.Check(Valid, passwordAvailable: false).Should().Equal(SmtpTransportProblem.PasswordMissing);

    [Theory]
    [InlineData("home@example.org", true)]
    [InlineData("Home <home@example.org>", true)]
    [InlineData("not-an-email", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsMailbox_matches_the_sender(string? address, bool expected)
        => SmtpTransportRules.IsMailbox(address).Should().Be(expected);
}
