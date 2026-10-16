using System.Text;
using FluentAssertions;
using MedReminder.Application.Abstractions;
using MedReminder.Infrastructure.Credentials;
using Xunit;

namespace MedReminder.Infrastructure.Tests.Credentials;

// Android backlog B5-01: the SMTP password store behind any host
// protector (the Android Keystore on the phone): only the protector's
// ciphertext is on disk, in smtp.protected of the given directory.
public sealed class PortableSmtpCredentialStoreTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "mr-smtp-" + Guid.NewGuid().ToString("N"));

    public void Dispose()
    {
        if (Directory.Exists(_directory)) Directory.Delete(_directory, recursive: true);
    }

    [Fact]
    public void The_password_is_stored_protected_and_read_back()
    {
        var store = SmtpCredentialStore.In(new ReversingProtector(), _directory);

        store.HasPassword.Should().BeFalse();
        store.GetPassword().Should().BeNull();
        store.SetPassword("P@ssw0rd!");

        var path = Path.Combine(_directory, SmtpCredentialStore.FileName);
        File.ReadAllText(path).Should().NotContain("P@ssw0rd!");
        File.Exists(path + ".tmp").Should().BeFalse();
        store.HasPassword.Should().BeTrue();
        store.GetPassword().Should().Be("P@ssw0rd!");
    }

    [Fact]
    public void A_new_password_replaces_the_stored_one_and_clear_removes_it()
    {
        var store = SmtpCredentialStore.In(new ReversingProtector(), _directory);
        store.SetPassword("first");
        store.SetPassword("second");

        store.GetPassword().Should().Be("second");

        store.Clear();
        store.HasPassword.Should().BeFalse();
        store.GetPassword().Should().BeNull();
        File.Exists(Path.Combine(_directory, SmtpCredentialStore.FileName)).Should().BeFalse();
    }

    [Fact]
    public void An_empty_password_is_refused()
    {
        var store = SmtpCredentialStore.In(new ReversingProtector(), _directory);

        FluentActions.Invoking(() => store.SetPassword("")).Should().Throw<ArgumentException>();
        store.HasPassword.Should().BeFalse();
    }

    // Stands in for DPAPI / Keystore: reversible, never the plain text.
    private sealed class ReversingProtector : ICredentialProtector
    {
        public string Protect(string plaintext)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(new string(plaintext.Reverse().ToArray())));

        public string Unprotect(string ciphertext)
            => new(Encoding.UTF8.GetString(Convert.FromBase64String(ciphertext)).Reverse().ToArray());
    }
}
