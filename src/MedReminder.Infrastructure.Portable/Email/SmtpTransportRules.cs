using MedReminder.Application.Abstractions;
using MimeKit;

namespace MedReminder.Infrastructure.Email;

// What a host's SMTP form checks before saving or testing (Android
// backlog B5-01): the same rules the MailKit sender relies on, so a
// configuration accepted by a form is one the sender can use. The
// desktop's NumericUpDown ranges (port 1-65535, timeout 5-300 s) are the
// reference.
public enum SmtpTransportProblem
{
    HostMissing,
    HostInvalid,
    PortOutOfRange,
    FromMissing,
    FromInvalid,
    TimeoutOutOfRange,
    // A user name without a password the sender can use (typed now or
    // already stored): every send would fail authentication.
    PasswordMissing,
}

public static class SmtpTransportRules
{
    public const int MinimumPort = 1;
    public const int MaximumPort = 65535;
    public const int MinimumTimeoutSeconds = 5;
    public const int MaximumTimeoutSeconds = 300;
    public const int DefaultPort = 587;
    public const int DefaultTimeoutSeconds = 30;

    // MimeKit accepts a bare local part (e.g. "not-an-email") as a valid
    // mailbox by default (ParserOptions.AllowAddressesWithoutDomain is
    // true). Every address the sender uses must be a full mailbox with a
    // domain, so it parses with that leniency turned off.
    internal static readonly ParserOptions AddressParserOptions = new()
    {
        AllowAddressesWithoutDomain = false,
    };

    // One full mailbox with a domain ("name@example.org", optionally with
    // a display name), as the sender parses it.
    public static bool IsMailbox(string? address)
    {
        if (string.IsNullOrWhiteSpace(address)) return false;
        return MailboxAddress.TryParse(AddressParserOptions, address.Trim(), out var mailbox)
               && mailbox.Address.Contains('@', StringComparison.Ordinal)
               && !mailbox.Address.EndsWith('@');
    }

    // passwordAvailable: a password was typed in the form or is already
    // stored and kept.
    public static IReadOnlyList<SmtpTransportProblem> Check(SmtpTransport transport, bool passwordAvailable)
    {
        ArgumentNullException.ThrowIfNull(transport);
        var problems = new List<SmtpTransportProblem>();

        var host = transport.Host?.Trim() ?? string.Empty;
        if (host.Length == 0) problems.Add(SmtpTransportProblem.HostMissing);
        else if (Uri.CheckHostName(host) == UriHostNameType.Unknown) problems.Add(SmtpTransportProblem.HostInvalid);

        if (transport.Port is < MinimumPort or > MaximumPort) problems.Add(SmtpTransportProblem.PortOutOfRange);

        if (string.IsNullOrWhiteSpace(transport.FromAddress)) problems.Add(SmtpTransportProblem.FromMissing);
        else if (!IsMailbox(transport.FromAddress)) problems.Add(SmtpTransportProblem.FromInvalid);

        if (transport.TimeoutSeconds is < MinimumTimeoutSeconds or > MaximumTimeoutSeconds)
        {
            problems.Add(SmtpTransportProblem.TimeoutOutOfRange);
        }

        if (!string.IsNullOrWhiteSpace(transport.Username) && !passwordAvailable)
        {
            problems.Add(SmtpTransportProblem.PasswordMissing);
        }
        return problems;
    }
}
