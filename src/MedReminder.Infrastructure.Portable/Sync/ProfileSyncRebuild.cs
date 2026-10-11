using System.Security.Cryptography;
using MedReminder.Application.Abstractions;
using MedReminder.Application.Sync;
using MedReminder.Infrastructure.Persistence;
using MedReminder.Infrastructure.Storage;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace MedReminder.Infrastructure.Sync;

// Rebuilds the open profile from its sync group with the stored key (B.1
// Phase 3d, docs/analysis/ANALYSIS-B1-MOBILE-SYNC.md §5.6, §5.7): after
// another device started a new generation (an import or a restore), or
// when this device was away so long that segments it needed were deleted.
// The group's database is built next to the profile database and swapped
// in like an import (ProfileDatabaseSwap); the host restarts afterwards.
// Same group, device and key. Operations of the old generation that this
// device had not published are lost, as §5.7 states.
//
// Shared by the desktop (ISyncSetupService.RebuildAsync) and the phone.
// Nothing is logged about the key.
public sealed class ProfileSyncRebuild
{
    private readonly string _databasePath;
    private readonly MedReminderDbContext _db;
    private readonly JoinSyncGroup _join;
    private readonly ISyncSettingsStore _settings;
    private readonly ISyncKeyStore _keys;
    private readonly ISyncTransportFactory _transports;
    private readonly IDatabaseExclusiveAccess _exclusiveAccess;
    private readonly TimeProvider _clock;
    private readonly ILogger<ProfileSyncRebuild>? _log;

    internal ProfileSyncRebuild(string databasePath, MedReminderDbContext db, JoinSyncGroup join,
        ISyncSettingsStore settings, ISyncKeyStore keys, ISyncTransportFactory transports,
        IDatabaseExclusiveAccess exclusiveAccess, TimeProvider clock, ILogger<ProfileSyncRebuild>? log)
    {
        _databasePath = databasePath;
        _db = db;
        _join = join;
        _settings = settings;
        _keys = keys;
        _transports = transports;
        _exclusiveAccess = exclusiveAccess;
        _clock = clock;
        _log = log;
    }

    // Returns the generation the profile was rebuilt from.
    // InvalidOperationException when sync is off, the key is not stored
    // here, or no generation of the group uses it (a key rotation: the
    // device needs the new key instead).
    public async Task<int> ExecuteAsync(CancellationToken cancellationToken)
    {
        var current = _settings.Load() ?? throw new InvalidOperationException("Sync is not enabled for this profile.");
        var key = _keys.Load(current.GroupId, current.KeyVersion)
            ?? throw new InvalidOperationException("The group key is not stored on this device.");
        var temp = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(_databasePath))!,
            $"medreminder.sync-{Guid.NewGuid():N}.db");
        try
        {
            var rebuilt = await _join.RejoinAsync(_transports.Create(SyncTarget.Of(current)), current, key, temp,
                cancellationToken);
            await ProfileDatabaseSwap.ReplaceAsync(_exclusiveAccess, _db, _databasePath, temp, _clock, cancellationToken);
            _settings.Save(rebuilt with { ResetPending = false });
            _log?.LogInformation("Profile rebuilt from sync generation {Generation}.", rebuilt.Generation);
            return rebuilt.Generation;
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
            if (File.Exists(temp))
            {
                SqliteConnection.ClearAllPools();
                try { File.Delete(temp); } catch (IOException) { } catch (UnauthorizedAccessException) { }
            }
        }
    }
}
