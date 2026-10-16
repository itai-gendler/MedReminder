using MailKit.Net.Smtp;
using MedReminder.Application.Abstractions;
using MedReminder.Application.Notifications;
using Microsoft.Extensions.Logging;

namespace MedReminder.Infrastructure.Email;

// Records what becomes of each email in the delivery journal (Android
// backlog B5-04, UI-16): Attempted when the send is handed over, then
// Accepted when the sender returns (MailKit returns once the server has
// accepted the message), Queued for an EmailNotSentException with
// NoNetwork, Failed with a problem category otherwise. A send the caller
// cancels stays Attempted: the outcome is unknown. The exception still
// reaches the caller, so the shared passes keep their own retry rules.
// A journal error is logged without detail and never fails the send.
// The host decides where it sits: around the sender that may throw the
// NoNetwork exception, inside any retry decorator so that one email is
// one entry.
public sealed class JournalingEmailNotificationService : IEmailNotificationService
{
    private readonly IEmailNotificationService _inner;
    private readonly IEmailDeliveryJournal _journal;
    private readonly TimeProvider _clock;
    private readonly string? _profileId;
    private readonly ILogger<JournalingEmailNotificationService>? _log;

    public JournalingEmailNotificationService(IEmailNotificationService inner, IEmailDeliveryJournal journal,
        TimeProvider clock, string? profileId, ILogger<JournalingEmailNotificationService>? log = null)
    {
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentNullException.ThrowIfNull(journal);
        ArgumentNullException.ThrowIfNull(clock);
        _inner = inner;
        _journal = journal;
        _clock = clock;
        _profileId = profileId;
        _log = log;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        var started = _clock.GetUtcNow();
        var entry = new EmailDeliveryEntry(Guid.NewGuid(), _profileId, message.Kind,
            message.ExplicitRecipient is not null, EmailDeliveryState.Attempted, null, started, started);
        Write(entry);
        try
        {
            await _inner.SendAsync(message, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            var problem = EmailDeliveryProblems.Classify(ex);
            var state = problem == EmailDeliveryProblem.NoNetwork ? EmailDeliveryState.Queued : EmailDeliveryState.Failed;
            Write(entry with { State = state, Problem = problem, At = _clock.GetUtcNow() });
            throw;
        }
        Write(entry with { State = EmailDeliveryState.Accepted, At = _clock.GetUtcNow() });
    }

    public Task<bool> TestConnectionAsync(CancellationToken cancellationToken)
        => _inner.TestConnectionAsync(cancellationToken);

    private void Write(EmailDeliveryEntry entry)
    {
        try
        {
            _journal.Record(entry);
        }
        catch (Exception ex)
        {
            _log?.LogWarning("Email delivery journal not written ({Error})", ex.GetType().Name);
        }
    }
}

// The problem category of a failed send: the precondition errors of the
// senders by their EmailNotSentException, the server's refusal of an
// address by MailKit's error code, the rest as the connection test
// classifies it (SmtpConnectionTester).
public static class EmailDeliveryProblems
{
    public static EmailDeliveryProblem Classify(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        return exception switch
        {
            EmailNotSentException notSent => notSent.Problem,
            SmtpCommandException { ErrorCode: SmtpErrorCode.RecipientNotAccepted } => EmailDeliveryProblem.RecipientRejected,
            SmtpCommandException { ErrorCode: SmtpErrorCode.SenderNotAccepted } => EmailDeliveryProblem.SenderRejected,
            _ => SmtpConnectionTester.Classify(exception) switch
            {
                SmtpTestOutcome.EncryptionUnavailable => EmailDeliveryProblem.EncryptionUnavailable,
                SmtpTestOutcome.NotConfigured => EmailDeliveryProblem.NotConfigured,
                SmtpTestOutcome.PasswordMissing => EmailDeliveryProblem.PasswordMissing,
                SmtpTestOutcome.HostNotFound => EmailDeliveryProblem.HostNotFound,
                SmtpTestOutcome.ConnectionFailed => EmailDeliveryProblem.ConnectionFailed,
                SmtpTestOutcome.TimedOut => EmailDeliveryProblem.TimedOut,
                SmtpTestOutcome.SecureConnectionFailed => EmailDeliveryProblem.SecureConnectionFailed,
                SmtpTestOutcome.AuthenticationFailed => EmailDeliveryProblem.AuthenticationFailed,
                SmtpTestOutcome.AuthenticationNotOffered => EmailDeliveryProblem.AuthenticationNotOffered,
                _ => EmailDeliveryProblem.Rejected,
            },
        };
    }
}
