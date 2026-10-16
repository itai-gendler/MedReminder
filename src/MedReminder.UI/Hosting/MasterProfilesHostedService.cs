using MedReminder.Application;
using MedReminder.Application.Abstractions;
using MedReminder.Application.Household;
using MedReminder.Infrastructure;
using MedReminder.Infrastructure.Cloud.GoogleDrive;
using MedReminder.Infrastructure.Cloud.OneDrive;
using MedReminder.Infrastructure.Household;
using MedReminder.Infrastructure.Profiles;
using MedReminder.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MedReminder.UI.Hosting;

// Household step H4c (docs/analysis/ANALYSIS-HOUSEHOLD-MASTER-DEVICE.md
// §7.6, C4): the active master evaluates every profile it holds, not only
// the open one. Every Master:ProfilesIntervalMinutes (default 15), for each
// other profile of this device it builds that profile's services (the same
// registrations as the application, with that profile's database, sync
// state and notification recipients, no on-screen notification), adopts a
// rotated group key, then runs the shared MasterProfileChecks: schema
// patches, sync, consumption catch-up, low-stock and dose checks (email
// only), sync again.
//
// Idle unless this device is the active master of an election (with no
// master elected, every device handles its open profile as before). The
// profiles run one after the other; an error in one is logged and the
// others go on. All writes happen in this process, which owns every
// profile database (single-instance mutex, CLAUDE.md §7); the static write
// gate serializes them with the open profile's.
internal sealed class MasterProfilesHostedService : BackgroundService
{
    private const int DefaultIntervalMinutes = 15;
    private static readonly TimeSpan StartDelay = TimeSpan.FromSeconds(90);

    private readonly IServiceProvider _services;
    private readonly ICurrentProfile _current;
    private readonly IProfileRegistry _registry;
    private readonly TimeProvider _clock;
    private readonly ILogger<MasterProfilesHostedService> _log;
    private readonly TimeSpan _interval;

    public MasterProfilesHostedService(IServiceProvider services, ICurrentProfile current, IProfileRegistry registry,
        TimeProvider clock, IConfiguration configuration, ILogger<MasterProfilesHostedService> log)
    {
        _services = services;
        _current = current;
        _registry = registry;
        _clock = clock;
        _log = log;
        var configured = int.TryParse(configuration["Master:ProfilesIntervalMinutes"], out var v) ? v : DefaultIntervalMinutes;
        _interval = TimeSpan.FromMinutes(Math.Max(1, configured));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(StartDelay, _clock, stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await RunOnceAsync(stoppingToken);
                await Task.Delay(_interval, _clock, stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Clean shutdown.
        }
    }

    internal async Task RunOnceAsync(CancellationToken cancellationToken)
    {
        MasterView view;
        await using (var scope = _services.CreateAsyncScope())
        {
            view = await scope.ServiceProvider.GetRequiredService<HouseholdMasterRole>().DescribeAsync(cancellationToken);
        }
        if (view.Master.Election is null || !view.SendsEmail) return;

        foreach (var profile in _registry.ListProfiles().Where(p => p.Id != _current.Id))
        {
            try
            {
                await RunProfileAsync(profile, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Master: the checks of profile {ProfileId} failed.", profile.Id);
            }
        }
    }

    private async Task RunProfileAsync(Profile profile, CancellationToken ct)
    {
        var current = new CurrentProfile(profile, AppDataPaths.GetProfilesRootDirectory());
        if (!File.Exists(current.DatabasePath)) return;

        await using var provider = ProfileServices.Build(_services, current);
        // Step H5b: a group rotated after a device removal; the new key comes
        // from the household grant.
        await ProfileServices.AdoptRotatedKeyAsync(provider, current, _log, ct);
        // The steps shared with the Android app (Infrastructure.Portable).
        var result = await MasterProfileChecks.RunAsync(provider, ct);
        if (result.SyncProblems > 0)
        {
            // The open profile's sync window repairs these; the checks went
            // on with what the database holds.
            _log.LogWarning("Master: the sync of profile {ProfileId} needs attention ({Problems} runs).",
                profile.Id, result.SyncProblems);
        }

        _log.LogInformation(
            "Master: profile {ProfileId} checked; consumption rows={Created}, low-stock notifications={Low}, dose reminders={Doses}.",
            profile.Id, result.ConsumptionRows, result.LowStockNotifications, result.DoseReminders);
    }
}
