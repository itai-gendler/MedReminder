using System.Security.Cryptography;
using MedReminder.Application.Abstractions;
using MedReminder.Application.Sync;
using MedReminder.Application.Sync.Remote;
using MedReminder.Infrastructure.Persistence;
using MedReminder.Infrastructure.Storage;
using Microsoft.Extensions.Logging;

namespace MedReminder.Infrastructure.Sync;

// ISyncSetupService for the desktop (B.1 Phase 3d). Join and rebuild
// build the group's database next to the profile database, then swap it
// in like an import (ProfileDatabaseSwap); the UI restarts the
// application. Nothing is logged about the passphrase or the key.
// Phase 4c: join with a pairing code, and the rekey after a key rotation
// on another device (ProfileSyncRekey, shared with the phone).
internal sealed class SyncSetupService : ISyncSetupService
{
    private readonly ICurrentProfile _profile;
    private readonly MedReminderDbContext _db;
    private readonly CreateSyncGroup _create;
    private readonly JoinSyncGroup _join;
    private readonly ISyncSettingsStore _settings;
    private readonly ISyncKeyStore _keys;
    private readonly ISyncTransportFactory _transports;
    private readonly TimeProvider _clock;
    private readonly IDatabaseExclusiveAccess _exclusiveAccess;
    private readonly ProfileSyncRebuild _rebuild;
    private readonly ProfileSyncRekey _rekey;
    private readonly ILogger<SyncSetupService> _log;

    public SyncSetupService(
        ICurrentProfile profile,
        MedReminderDbContext db,
        CreateSyncGroup create,
        JoinSyncGroup join,
        ISyncSettingsStore settings,
        ISyncKeyStore keys,
        ISyncTransportFactory transports,
        TimeProvider clock,
        IDatabaseExclusiveAccess exclusiveAccess,
        ProfileSyncRebuild rebuild,
        ProfileSyncRekey rekey,
        ILogger<SyncSetupService> log)
    {
        _profile = profile;
        _db = db;
        _create = create;
        _join = join;
        _settings = settings;
        _keys = keys;
        _transports = transports;
        _clock = clock;
        _exclusiveAccess = exclusiveAccess;
        _rebuild = rebuild;
        _rekey = rekey;
        _log = log;
    }

    public Task<IReadOnlyList<Guid>> ListGroupsAsync(SyncTarget target, CancellationToken cancellationToken)
        => JoinSyncGroup.ListGroupsAsync(_transports.Create(target), cancellationToken);

    public Task<IReadOnlyList<SyncGroupCandidate>> FindGroupsAsync(SyncTarget target, char[] passphrase,
        CancellationToken cancellationToken)
        => _join.FindGroupsAsync(_transports.Create(target), passphrase, cancellationToken);

    public async Task CreateAsync(SyncTarget target, char[] passphrase, string deviceName, CancellationToken cancellationToken)
    {
        var settings = await _create.ExecuteAsync(_transports.Create(target), passphrase, target,
            cancellationToken, deviceName: deviceName);
        _log.LogInformation("Sync enabled for profile {ProfileId} with group {GroupId}.", _profile.Id, settings.GroupId);
    }

    public Task JoinAsync(SyncTarget target, Guid groupId, char[] passphrase, string deviceName,
        CancellationToken cancellationToken)
        => JoinCoreAsync(target, groupId, new SyncKeySource.Passphrase(passphrase), deviceName, cancellationToken);

    public Task JoinWithPairingAsync(SyncTarget target, SyncPairingCode code, string deviceName,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(code);
        return JoinCoreAsync(target, code.GroupId, new SyncKeySource.Pairing(code), deviceName, cancellationToken);
    }

    private async Task JoinCoreAsync(SyncTarget target, Guid groupId, SyncKeySource source, string deviceName,
        CancellationToken cancellationToken)
    {
        if (_settings.Load() is not null) throw new InvalidOperationException("Sync is already enabled.");
        var temp = TempPath();
        try
        {
            var result = await _join.ExecuteAsync(_transports.Create(target), groupId, source, temp,
                target, cancellationToken, deviceName);
            try
            {
                await ProfileDatabaseSwap.ReplaceAsync(
                    _exclusiveAccess, _db, _profile.DatabasePath, temp, _clock, cancellationToken);
                _keys.Save(result.Settings.GroupId, result.Settings.KeyVersion, result.Key);
                _settings.Save(result.Settings);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(result.Key);
            }
            _log.LogInformation("Profile {ProfileId} joined sync group {GroupId}.", _profile.Id, groupId);
        }
        finally
        {
            DeleteIfPresent(temp);
        }
    }

    // Shared with the phone (ProfileSyncRebuild).
    public async Task RebuildAsync(CancellationToken cancellationToken)
    {
        var generation = await _rebuild.ExecuteAsync(cancellationToken);
        _log.LogInformation("Profile {ProfileId} rebuilt from sync generation {Generation}.", _profile.Id, generation);
    }

    // Shared with the phone (ProfileSyncRekey).
    public async Task<(int Carried, int Dropped)> RekeyAsync(SyncKeySource source, CancellationToken cancellationToken)
    {
        var (carried, dropped) = await _rekey.ExecuteAsync(source, cancellationToken);
        var settings = _settings.Load();
        _log.LogInformation(
            "Profile {ProfileId} took key version {KeyVersion} and generation {Generation}: " +
            "{Carried} own operations carried over, {Dropped} dropped.",
            _profile.Id, settings?.KeyVersion, settings?.Generation, carried, dropped);
        return (carried, dropped);
    }

    private string TempPath()
        => Path.Combine(_profile.DataDirectory, $"medreminder.sync-{Guid.NewGuid():N}.db");

    private static void DeleteIfPresent(string path)
    {
        if (!File.Exists(path)) return;
        try { File.Delete(path); } catch { /* best effort */ }
    }
}
