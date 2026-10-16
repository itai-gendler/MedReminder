using MedReminder.Application.Abstractions;
using MedReminder.Domain.Sync;
using Microsoft.Extensions.Options;

namespace MedReminder.Infrastructure.Profiles;

// The profile's notification recipients as IOptionsMonitor, for a host
// without the desktop's reloading configuration (Android backlog B5-03):
// each read takes the profile's notifications.settings.json as it is now,
// through IProfileSettingsStore, so a value saved in this session or
// replicated from another device is what the next email uses. The
// desktop binds the same file through its configuration instead.
public sealed class ProfileNotificationSettingsMonitor : IOptionsMonitor<NotificationSettings>
{
    private readonly IProfileSettingsStore _store;

    public ProfileNotificationSettingsMonitor(IProfileSettingsStore store)
    {
        ArgumentNullException.ThrowIfNull(store);
        _store = store;
    }

    public NotificationSettings CurrentValue
    {
        get
        {
            var stored = _store.Read();
            string Value(string key) => stored.GetValueOrDefault(key) ?? string.Empty;
            return new NotificationSettings
            {
                ToAddress = Value(ProfileSetting.ToAddress),
                CaregiverAddress = Value(ProfileSetting.CaregiverAddress),
                DoctorAddress = Value(ProfileSetting.DoctorAddress),
                CaregiverEmails = Value(ProfileSetting.CaregiverEmails),
                CaregiverDigest = Value(ProfileSetting.CaregiverDigest),
                CaregiverDigestSentOn = Value(ProfileSetting.CaregiverDigestSentOn),
                PackageExpiryLeadDays = Value(ProfileSetting.PackageExpiryLeadDays),
                PackageInUseLeadDays = Value(ProfileSetting.PackageInUseLeadDays),
                Region = Value(ProfileSetting.Region),
            };
        }
    }

    public NotificationSettings Get(string? name) => CurrentValue;

    public IDisposable? OnChange(Action<NotificationSettings, string?> listener) => null;
}
