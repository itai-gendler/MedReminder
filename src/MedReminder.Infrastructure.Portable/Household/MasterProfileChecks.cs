using MedReminder.Application.Abstractions;
using MedReminder.Application.Monitoring;
using MedReminder.Application.Sync;
using MedReminder.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace MedReminder.Infrastructure.Household;

// What the checks of one profile did. SyncProblems counts the sync runs
// that need attention (problems, a rebuild, a new key or a newer
// generation): the profile's own sync screen repairs them when it is
// opened; here the checks go on with what the database holds.
public sealed record MasterProfileCheckResult(
    bool Synced,
    int ConsumptionRows,
    int LowStockNotifications,
    int DoseReminders,
    int SyncProblems);

// Household step H4c (docs/analysis/ANALYSIS-HOUSEHOLD-MASTER-DEVICE.md
// §7.6, C4): the checks the active master runs for a profile that is not
// open on it, over that profile's services (its database, sync state and
// notification recipients, built by the host):
//
//   1. applies the schema patches (DatabaseInitializer);
//   2. syncs the profile's group, so the stock is current;
//   3. runs the consumption catch-up and the low-stock and dose checks;
//   4. syncs again, so the other devices learn the emails sent
//      (EmailNotificationSent).
//
// Shared by the desktop (MasterProfilesHostedService) and the Android app
// (backlog B5-03). The host decides when to run, gives the services a
// notifier that shows nothing (an on-screen reminder for a profile nobody
// opened would reach the wrong person, so these checks send email only),
// and runs only while IMasterRole says this device sends email.
public static class MasterProfileChecks
{
    public static async Task<MasterProfileCheckResult> RunAsync(IServiceProvider profile,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(profile);
        await Scoped(profile, async sp =>
        {
            await sp.GetRequiredService<DatabaseInitializer>().InitializeAsync(cancellationToken);
            return true;
        });
        var synced = await Scoped(profile, sp => Task.FromResult(sp.GetRequiredService<ISyncSettingsStore>().Load() is not null));
        var problems = 0;
        if (synced && !await SyncAsync(profile, cancellationToken)) problems++;

        var created = await Scoped(profile, sp => sp.GetRequiredService<ConsumptionCatchUp>().RunAsync(cancellationToken));
        var low = await Scoped(profile, sp => sp.GetRequiredService<MedicationMonitor>().RunAsync(cancellationToken));
        var doses = await Scoped(profile, sp => sp.GetRequiredService<DoseReminderService>().RunAsync(cancellationToken));
        if (synced && !await SyncAsync(profile, cancellationToken)) problems++;

        return new MasterProfileCheckResult(synced, created, low.NotificationsSent, doses.FiredCount, problems);
    }

    // True when the run needs no attention.
    private static Task<bool> SyncAsync(IServiceProvider profile, CancellationToken cancellationToken)
        => Scoped(profile, async sp =>
        {
            var result = await sp.GetRequiredService<SyncEngine>().RunAsync(cancellationToken);
            return result.Problems.Count == 0 && !result.RebuildRequired && !result.NewKeyRequired
                && result.NewerGeneration is null;
        });

    private static async Task<T> Scoped<T>(IServiceProvider provider, Func<IServiceProvider, Task<T>> action)
    {
        await using var scope = provider.CreateAsyncScope();
        return await action(scope.ServiceProvider);
    }
}
