using System.Net.Sockets;
using FluentAssertions;
using MailKit.Net.Smtp;
using MedReminder.Application.Abstractions;
using MedReminder.Application.Notifications;
using MedReminder.Infrastructure.Email;
using MimeKit;
using Xunit;

namespace MedReminder.Infrastructure.Tests.Email;

// The email delivery journal and its recording decorator (Android backlog
// B5-04): queued, attempted, failed and accepted states, without any
// subject, body or address on disk.
public sealed class EmailDeliveryJournalTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "mr-journal-" + Guid.NewGuid().ToString("N"));
    private readonly ManualClock _clock = new(new DateTimeOffset(2026, 10, 16, 9, 0, 0, TimeSpan.Zero));

    public void Dispose()
    {
        if (Directory.Exists(_directory)) Directory.Delete(_directory, recursive: true);
    }

    private JsonFileEmailDeliveryJournal Journal() => new(_directory, _clock);

    private JournalingEmailNotificationService Sut(Func<EmailMessage, Task> send, string? profile = "p1")
        => new(new ScriptedSender(send), Journal(), _clock, profile);

    private static EmailMessage LowStock() => new("Low stock: Medicine X", "Body naming Medicine X",
        Kind: EmailKind.LowStock);

    [Fact]
    public async Task An_accepted_send_is_recorded_as_accepted_by_the_server()
    {
        var sut = Sut(_ =>
        {
            _clock.Advance(TimeSpan.FromSeconds(3));
            return Task.CompletedTask;
        });

        await sut.SendAsync(LowStock(), CancellationToken.None);

        var entry = Journal().Read().Should().ContainSingle().Subject;
        entry.State.Should().Be(EmailDeliveryState.Accepted);
        entry.Problem.Should().BeNull();
        entry.Kind.Should().Be(EmailKind.LowStock);
        entry.ProfileId.Should().Be("p1");
        entry.Request.Should().BeFalse();
        (entry.At - entry.StartedAt).Should().Be(TimeSpan.FromSeconds(3));
    }

    [Fact]
    public async Task The_attempt_is_recorded_before_the_server_answers()
    {
        EmailDeliveryEntry? during = null;
        var sut = Sut(_ =>
        {
            during = Journal().Read().Single();
            return Task.CompletedTask;
        });

        await sut.SendAsync(LowStock(), CancellationToken.None);

        during!.State.Should().Be(EmailDeliveryState.Attempted);
    }

    [Fact]
    public async Task No_network_is_queued_and_the_exception_reaches_the_caller()
    {
        var sut = Sut(_ => throw new EmailNotSentException(EmailDeliveryProblem.NoNetwork, "offline"));

        await FluentActions.Awaiting(() => sut.SendAsync(LowStock(), CancellationToken.None))
            .Should().ThrowAsync<EmailNotSentException>();

        var entry = Journal().Read().Single();
        entry.State.Should().Be(EmailDeliveryState.Queued);
        entry.Problem.Should().Be(EmailDeliveryProblem.NoNetwork);
    }

    [Fact]
    public async Task A_refused_recipient_is_a_failure_with_its_category()
    {
        var sut = Sut(_ => throw new SmtpCommandException(SmtpErrorCode.RecipientNotAccepted,
            SmtpStatusCode.MailboxUnavailable, new MailboxAddress("", "someone@example.org"), "550 no such user"));

        await FluentActions.Awaiting(() => sut.SendAsync(LowStock(), CancellationToken.None))
            .Should().ThrowAsync<SmtpCommandException>();

        var entry = Journal().Read().Single();
        entry.State.Should().Be(EmailDeliveryState.Failed);
        entry.Problem.Should().Be(EmailDeliveryProblem.RecipientRejected);
    }

    [Theory]
    [InlineData(SocketError.HostNotFound, EmailDeliveryProblem.HostNotFound)]
    [InlineData(SocketError.ConnectionRefused, EmailDeliveryProblem.ConnectionFailed)]
    [InlineData(SocketError.TimedOut, EmailDeliveryProblem.TimedOut)]
    public void Connection_errors_are_classified_as_the_connection_test_does(SocketError error,
        EmailDeliveryProblem expected)
        => EmailDeliveryProblems.Classify(new SocketException((int)error)).Should().Be(expected);

    [Fact]
    public void Sender_refusal_and_other_errors_are_classified()
    {
        EmailDeliveryProblems.Classify(new SmtpCommandException(SmtpErrorCode.SenderNotAccepted,
                SmtpStatusCode.MailboxNameNotAllowed, new MailboxAddress("", "me@example.org"), "553"))
            .Should().Be(EmailDeliveryProblem.SenderRejected);
        EmailDeliveryProblems.Classify(new SmtpCommandException(SmtpErrorCode.MessageNotAccepted,
                SmtpStatusCode.TransactionFailed, "554"))
            .Should().Be(EmailDeliveryProblem.Rejected);
        EmailDeliveryProblems.Classify(new MailKit.Security.AuthenticationException())
            .Should().Be(EmailDeliveryProblem.AuthenticationFailed);
        EmailDeliveryProblems.Classify(new SmtpEncryptionRequiredException())
            .Should().Be(EmailDeliveryProblem.EncryptionUnavailable);
    }

    [Fact]
    public async Task The_MailKit_sender_names_its_precondition_problems()
    {
        var journal = Journal();
        var smtp = new MailKitEmailNotificationService(
            new Monitor<SmtpSettings>(new SmtpSettings()),
            new Monitor<NotificationSettings>(new NotificationSettings { ToAddress = "user@example.org" }),
            new NoCredentials(), Microsoft.Extensions.Logging.Abstractions.NullLogger<MailKitEmailNotificationService>.Instance);
        var sut = new JournalingEmailNotificationService(smtp, journal, _clock, null);

        await FluentActions.Awaiting(() => sut.SendAsync(LowStock(), CancellationToken.None))
            .Should().ThrowAsync<InvalidOperationException>();

        var entry = journal.Read().Single();
        entry.State.Should().Be(EmailDeliveryState.Failed);
        entry.Problem.Should().Be(EmailDeliveryProblem.NotConfigured);
    }

    [Fact]
    public async Task A_cancelled_send_stays_attempted_and_reads_as_unfinished_later()
    {
        using var cancel = new CancellationTokenSource();
        var sut = Sut(_ =>
        {
            cancel.Cancel();
            cancel.Token.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        });

        await FluentActions.Awaiting(() => sut.SendAsync(LowStock(), cancel.Token))
            .Should().ThrowAsync<OperationCanceledException>();

        var entry = Journal().Read().Single();
        entry.State.Should().Be(EmailDeliveryState.Attempted);
        entry.IsUnfinished(_clock.GetUtcNow(), TimeSpan.FromMinutes(10)).Should().BeFalse();
        _clock.Advance(TimeSpan.FromMinutes(11));
        entry.IsUnfinished(_clock.GetUtcNow(), TimeSpan.FromMinutes(10)).Should().BeTrue();
    }

    [Fact]
    public async Task No_subject_body_or_address_is_written_to_disk()
    {
        var sut = Sut(_ => Task.CompletedTask);
        await sut.SendAsync(LowStock() with { ExplicitRecipient = "doctor@example.org" }, CancellationToken.None);

        var text = await File.ReadAllTextAsync(Path.Combine(_directory, JsonFileEmailDeliveryJournal.FileName));
        text.Should().NotContain("Medicine X").And.NotContain("example.org");
        Journal().Read().Single().Request.Should().BeTrue();
    }

    [Fact]
    public void The_journal_keeps_the_newest_entries_within_size_and_age()
    {
        var journal = Journal();
        var old = _clock.GetUtcNow();
        journal.Record(Entry(old));
        _clock.Advance(JsonFileEmailDeliveryJournal.MaxAge + TimeSpan.FromDays(1));
        for (var i = 0; i < JsonFileEmailDeliveryJournal.MaxEntries + 5; i++)
        {
            journal.Record(Entry(_clock.GetUtcNow().AddMinutes(i)));
        }

        var entries = journal.Read();
        entries.Should().HaveCount(JsonFileEmailDeliveryJournal.MaxEntries);
        entries.Should().NotContain(e => e.StartedAt == old);
        entries.Should().BeInDescendingOrder(e => e.StartedAt);
    }

    [Fact]
    public void An_unreadable_file_reads_as_empty_and_is_replaced()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(Path.Combine(_directory, JsonFileEmailDeliveryJournal.FileName), "{ not json");
        var journal = Journal();

        journal.Read().Should().BeEmpty();
        journal.Record(Entry(_clock.GetUtcNow()));
        journal.Read().Should().ContainSingle();
    }

    [Fact]
    public async Task A_journal_that_throws_never_fails_the_send()
    {
        var sent = false;
        var sut = new JournalingEmailNotificationService(new ScriptedSender(_ =>
        {
            sent = true;
            return Task.CompletedTask;
        }), new BrokenJournal(), _clock, null);

        await sut.SendAsync(LowStock(), CancellationToken.None);

        sent.Should().BeTrue();
    }

    private static EmailDeliveryEntry Entry(DateTimeOffset at) => new(Guid.NewGuid(), "p1", EmailKind.Digest, false,
        EmailDeliveryState.Accepted, null, at, at);

    private sealed class ScriptedSender(Func<EmailMessage, Task> send) : IEmailNotificationService
    {
        public Task SendAsync(EmailMessage message, CancellationToken cancellationToken) => send(message);

        public Task<bool> TestConnectionAsync(CancellationToken cancellationToken) => Task.FromResult(true);
    }

    private sealed class BrokenJournal : IEmailDeliveryJournal
    {
        public void Record(EmailDeliveryEntry entry) => throw new IOException("disk full");

        public IReadOnlyList<EmailDeliveryEntry> Read() => [];
    }

    private sealed class ManualClock(DateTimeOffset now) : TimeProvider
    {
        private DateTimeOffset _now = now;

        public override DateTimeOffset GetUtcNow() => _now;

        public void Advance(TimeSpan by) => _now += by;
    }

    private sealed class Monitor<T>(T value) : Microsoft.Extensions.Options.IOptionsMonitor<T>
    {
        public T CurrentValue => value;

        public T Get(string? name) => value;

        public IDisposable? OnChange(Action<T, string?> listener) => null;
    }

    private sealed class NoCredentials : ISmtpCredentialStore
    {
        public bool HasPassword => false;

        public string? GetPassword() => null;

        public void SetPassword(string password)
        {
        }

        public void Clear()
        {
        }
    }
}
