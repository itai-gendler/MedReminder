namespace MedReminder.Application.Notifications;

// What became of an email the app tried to send (Android backlog B5-04,
// UI-16): the states a screen may show, none of which claims delivery to
// the mailbox. Stored by name in the delivery journal: do not rename.
public enum EmailDeliveryState
{
    // Not handed to the server yet, for a reason that can pass by itself
    // (no network): a later check can produce the email again.
    Queued,
    // The connection to the server started and no answer was recorded:
    // still running, or the process stopped before the server answered,
    // so the outcome is unknown.
    Attempted,
    // Not accepted: the configuration is incomplete, the server could not
    // be reached or it refused the message.
    Failed,
    // The SMTP server accepted the message. Delivery to the mailbox is not
    // known: the server can still bounce it.
    Accepted,
}

// Why an email was queued or failed, in categories a screen can turn into
// recovery guidance. Stored by name: do not rename.
public enum EmailDeliveryProblem
{
    // Queued: the device has no network.
    NoNetwork,
    // Host or sender address missing.
    NotConfigured,
    // No recipient for the profile (or no caregiver for the weekly summary).
    RecipientMissing,
    // A user name without a stored password.
    PasswordMissing,
    // The connection stayed plain where encryption is required.
    EncryptionUnavailable,
    HostNotFound,
    ConnectionFailed,
    TimedOut,
    SecureConnectionFailed,
    AuthenticationFailed,
    AuthenticationNotOffered,
    // The server refused a recipient address.
    RecipientRejected,
    // The server refused the sender address.
    SenderRejected,
    // The server refused the message or answered a command with an error.
    Rejected,
}

// One email in the journal. No subject, body or address is kept: the
// subject names medicines and the addresses are personal data. ProfileId
// is the profile the email was for (null when the host does not say);
// Request marks a user-confirmed send to an explicit recipient
// (prescription request). StartedAt is when the send was handed over, At
// when the state was last written.
public sealed record EmailDeliveryEntry(
    Guid Id,
    string? ProfileId,
    EmailKind? Kind,
    bool Request,
    EmailDeliveryState State,
    EmailDeliveryProblem? Problem,
    DateTimeOffset StartedAt,
    DateTimeOffset At)
{
    // An attempt with no answer after `limit` is no longer running: the
    // process stopped (Android ended the job or the receiver) and the
    // outcome is unknown.
    public bool IsUnfinished(DateTimeOffset now, TimeSpan limit)
        => State == EmailDeliveryState.Attempted && now - At > limit;
}

// The recent emails of the installation on this device, newest first,
// bounded in size and age by the implementation. Record inserts an entry
// or replaces the one with the same Id. Neither method throws for an
// unreadable or unwritable file: the journal is informative, and a send
// never fails because of it.
public interface IEmailDeliveryJournal
{
    void Record(EmailDeliveryEntry entry);
    IReadOnlyList<EmailDeliveryEntry> Read();
}

// The email was not sent, for a known reason, before anything reached the
// server: NoNetwork is a queued email, the other problems are failures.
// An InvalidOperationException, as the precondition errors it replaces,
// so existing handlers keep catching it. The message names no address.
public class EmailNotSentException : InvalidOperationException
{
    public EmailNotSentException(EmailDeliveryProblem problem, string message)
        : base(message)
    {
        Problem = problem;
    }

    public EmailDeliveryProblem Problem { get; }
}
