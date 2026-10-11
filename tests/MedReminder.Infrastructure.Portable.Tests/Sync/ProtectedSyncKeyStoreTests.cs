using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using MedReminder.Application.Abstractions;
using MedReminder.Infrastructure;
using MedReminder.Infrastructure.Household;
using MedReminder.Infrastructure.Sync;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MedReminder.Infrastructure.Portable.Tests.Sync;

// The portable group key stores of a host without DPAPI (B2-05): the key
// protected by the host's protector, read back only for its group and
// version, and found for any profile by ProfileGroupKeys.
public sealed class ProtectedSyncKeyStoreTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "mr-synckeys-" + Guid.NewGuid().ToString("N"));
    private readonly XorProtector _protector = new();

    public void Dispose()
    {
        try { Directory.Delete(_root, recursive: true); } catch (IOException) { }
    }

    [Fact]
    public void The_key_is_stored_protected_and_read_back_for_its_group_and_version_only()
    {
        var group = Guid.NewGuid();
        var key = RandomNumberGenerator.GetBytes(32);
        var store = ProtectedSyncKeyStore.In(_protector, Path.Combine(_root, "p1"));

        store.Load(group, 1).Should().BeNull();
        store.Save(group, 1, key);

        store.Load(group, 1).Should().Equal(key);
        store.Load(group, 2).Should().BeNull();
        store.Load(Guid.NewGuid(), 1).Should().BeNull();
        File.ReadAllText(Path.Combine(_root, "p1", ProtectedSyncKeyStore.FileName))
            .Should().NotContain(Convert.ToBase64String(key));

        store.Clear();
        store.Load(group, 1).Should().BeNull();
    }

    [Fact]
    public void ProfileGroupKeys_reads_the_settings_and_the_key_of_any_profile()
    {
        var group = Guid.NewGuid();
        var key = RandomNumberGenerator.GetBytes(32);
        var directory = Path.Combine(_root, "abc");
        Directory.CreateDirectory(directory);
        new JsonSyncSettingsStore(Path.Combine(directory, JsonSyncSettingsStore.FileName))
            .Save(new SyncSettings(group, Guid.NewGuid(), Generation: 1, KeyVersion: 3, Folder: "/x"));
        ProtectedSyncKeyStore.In(_protector, directory).Save(group, 3, key);
        var keys = new ProfileGroupKeys(_root, d => ProtectedSyncKeyStore.In(_protector, d));

        keys.Load("abc").Should().BeEquivalentTo(new ProfileGroupKey(group, 3, key));
        keys.Load("missing").Should().BeNull();
        keys.Load("..").Should().BeNull();
    }

    [Fact]
    public void The_portable_registration_uses_the_hosts_protector()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ICredentialProtector>(_protector);
        services.AddMedReminderPortableSyncKeys(_root, Path.Combine(_root, "current"));
        using var provider = services.BuildServiceProvider();

        var group = Guid.NewGuid();
        provider.GetRequiredService<ISyncKeyStore>().Save(group, 1, [1, 2, 3]);

        ProtectedSyncKeyStore.In(_protector, Path.Combine(_root, "current")).Load(group, 1).Should().Equal(1, 2, 3);
        provider.GetRequiredService<IProfileGroupKeys>().Should().BeOfType<ProfileGroupKeys>();
        provider.GetRequiredService<IHouseholdProfileInstaller>().Should().BeOfType<HouseholdProfileInstaller>();
    }

    private sealed class XorProtector : ICredentialProtector
    {
        public string Protect(string plaintext)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(plaintext).Select(b => (byte)(b ^ 0x5A)).ToArray());

        public string Unprotect(string ciphertext)
            => Encoding.UTF8.GetString(Convert.FromBase64String(ciphertext).Select(b => (byte)(b ^ 0x5A)).ToArray());
    }
}
