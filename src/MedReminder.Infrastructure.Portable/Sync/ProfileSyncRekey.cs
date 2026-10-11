using System.Security.Cryptography;
using MedReminder.Application.Abstractions;
using MedReminder.Application.Sync;
using MedReminder.Infrastructure.Persistence;
using MedReminder.Infrastructure.Storage;
using Microsoft.Data.Sqlite;

namespace MedReminder.Infrastructure.Sync;

// Takes a new group key for the open profile (B.1 Phase 4c, §6.2; household
// step H5b): after a key rotation on another device, from the new
// passphrase, a pairing code or a household grant (SyncKeySource.Known).
// The profile is rebuilt from the generation sealed with the new key and
// swapped in like an import (ProfileDatabaseSwap), then this device's own
// operations of the previous generation are carried over, so nothing
// recorded here is lost (ApplyRemoteOperations.ApplyCarriedAsync). Same
// group and device. The host restarts afterwards.
//
// Shared by the desktop (ISyncSetupService.RekeyAsync) and the phone.
// Nothing is logged about the key.
public sealed class ProfileSyncRekey
{
    private readonly string _databasePath;
    private readonly MedReminderDbContext _db;
    private readonly JoinSyncGroup _join;
    private readonly ISyncSettingsStore _settings;
    private readonly ISyncKeyStore _keys;
    private readonly ISyncTransportFactory _transports;
    private readonly ISyncOperationRepository _operations;
    private readonly ApplyRemoteOperations _apply;
    private readonly IDatabaseExclusiveAccess _exclusiveAccess;
    private readonly TimeProvider _clock;

    internal ProfileSyncRekey(string databasePath, MedReminderDbContext db, JoinSyncGroup join,
        ISyncSettingsStore settings, ISyncKeyStore keys, ISyncTransportFactory transports,
        ISyncOperationRepository operations, ApplyRemoteOperations apply, IDatabaseExclusiveAccess exclusiveAccess,
        TimeProvider clock)
    {
        _databasePath = databasePath;
        _db = db;
        _join = join;
        _settings = settings;
        _keys = keys;
        _transports = transports;
        _operations = operations;
        _apply = apply;
        _exclusiveAccess = exclusiveAccess;
        _clock = clock;
    }

    // Returns the own operations carried over and those dropped (for a
    // medicine the new generation does not have). CryptographicException
    // for a wrong passphrase; the pairing exceptions for a code no longer
    // offered; InvalidOperationException when sync is off.
    public async Task<(int Carried, int Dropped)> ExecuteAsync(SyncKeySource source, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(source);
        var current = _settings.Load() ?? throw new InvalidOperationException("Sync is not enabled for this profile.");
        // Read before the swap: the new database holds only what the
        // rotating device had.
        var own = (await _operations.ListAllAsync(cancellationToken))
            .Where(o => o.DeviceId == current.DeviceId && o.Generation == current.Generation)
            .ToList();
        var temp = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(_databasePath))!,
            $"medreminder.sync-{Guid.NewGuid():N}.db");
        try
        {
            var result = await _join.RekeyAsync(_transports.Create(SyncTarget.Of(current)), current, source, temp,
                cancellationToken);
            try
            {
                await ProfileDatabaseSwap.ReplaceAsync(_exclusiveAccess, _db, _databasePath, temp, _clock, cancellationToken);
                // Nothing read from the replaced file may be written back.
                _db.ChangeTracker.Clear();
                _keys.Save(result.Settings.GroupId, result.Settings.KeyVersion, result.Key);
                _settings.Save(result.Settings);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(result.Key);
            }
            return await _apply.ApplyCarriedAsync(own, cancellationToken);
        }
        finally
        {
            if (File.Exists(temp))
            {
                SqliteConnection.ClearAllPools();
                try { File.Delete(temp); } catch (IOException) { } catch (UnauthorizedAccessException) { }
            }
        }
    }
}
