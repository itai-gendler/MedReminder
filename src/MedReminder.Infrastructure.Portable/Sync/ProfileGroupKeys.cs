using MedReminder.Application.Abstractions;

namespace MedReminder.Infrastructure.Sync;

// IProfileGroupKeys over profiles\<id>\sync.settings.json and the profile's
// protected group key (household step H3b), the files the sync of the open
// profile uses, read here for any profile of the installation. The key
// store is the host's: DPAPI on Windows, ProtectedSyncKeyStore on mobile.
public sealed class ProfileGroupKeys : IProfileGroupKeys
{
    private readonly string _profilesRoot;
    private readonly Func<string, ISyncKeyStore> _keyStoreIn;

    // keyStoreIn: the key store of a profile folder.
    public ProfileGroupKeys(string profilesRoot, Func<string, ISyncKeyStore> keyStoreIn)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(profilesRoot);
        ArgumentNullException.ThrowIfNull(keyStoreIn);
        _profilesRoot = profilesRoot;
        _keyStoreIn = keyStoreIn;
    }

    public ProfileGroupKey? Load(string profileId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(profileId);
        // A profile id names a folder; it must not name one outside the
        // profiles folder.
        if (profileId.IndexOfAny([.. Path.GetInvalidFileNameChars(), '.']) >= 0) return null;
        var directory = Path.Combine(_profilesRoot, profileId);
        if (!Directory.Exists(directory)) return null;
        var settings = new JsonSyncSettingsStore(Path.Combine(directory, JsonSyncSettingsStore.FileName)).Load();
        if (settings is null) return null;
        var key = _keyStoreIn(directory).Load(settings.GroupId, settings.KeyVersion);
        return key is null ? null : new ProfileGroupKey(settings.GroupId, settings.KeyVersion, key);
    }
}
