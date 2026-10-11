using System.Text.Json;
using MedReminder.Application.Abstractions;

namespace MedReminder.Infrastructure.Sync;

// The group key of a profile's sync group in profiles\<id>\sync.protected,
// protected with the device's credential protector (docs/analysis/
// ANALYSIS-B1-MOBILE-SYNC.md §5.4, §7.5): the mobile counterpart of the
// desktop's DPAPI store (Android Keystore through the host's protector).
// Only the key of the version in use is kept, as on the desktop.
public sealed class ProtectedSyncKeyStore : ISyncKeyStore
{
    public const string FileName = "sync.protected";

    private readonly ICredentialProtector _protector;
    private readonly string _path;

    public ProtectedSyncKeyStore(ICredentialProtector protector, string path)
    {
        ArgumentNullException.ThrowIfNull(protector);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        _protector = protector;
        _path = path;
    }

    // The store of the profile folder `directory`.
    public static ProtectedSyncKeyStore In(ICredentialProtector protector, string directory)
        => new(protector, Path.Combine(directory, FileName));

    public byte[]? Load(Guid groupId, int keyVersion)
    {
        if (!File.Exists(_path)) return null;
        var stored = JsonSerializer.Deserialize<StoredKey>(File.ReadAllText(_path));
        if (stored is null || stored.GroupId != groupId || stored.KeyVersion != keyVersion) return null;
        return Convert.FromBase64String(_protector.Unprotect(stored.Key));
    }

    public void Save(Guid groupId, int keyVersion, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(key);
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(_path))!);
        var temporary = _path + ".tmp";
        File.WriteAllText(temporary, JsonSerializer.Serialize(
            new StoredKey(groupId, keyVersion, _protector.Protect(Convert.ToBase64String(key)))));
        File.Move(temporary, _path, overwrite: true);
    }

    public void Clear()
    {
        if (File.Exists(_path)) File.Delete(_path);
    }

    private sealed record StoredKey(Guid GroupId, int KeyVersion, string Key);
}
