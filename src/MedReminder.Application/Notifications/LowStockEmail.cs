using MedReminder.Application.Abstractions;
using MedReminder.Application.UseCases;
using MedReminder.Domain.Notifications;

namespace MedReminder.Application.Notifications;

// Turns the low-stock email of medicines on or off (Android backlog
// B5-02, UI-16): the Email flag of NotificationChannels, the same flag the
// desktop's medicine form sets, written through UpdateMedicine with the
// stored values as baseline, so only the channels are recorded and
// replicated. The other channel (the device notice) is kept as stored.
// A medicine already as wanted, or deleted meanwhile, is skipped. Returns
// how many medicines changed.
public sealed class UpdateLowStockEmail
{
    private readonly IMedicineRepository _medicines;
    private readonly UpdateMedicine _update;

    public UpdateLowStockEmail(IMedicineRepository medicines, UpdateMedicine update)
    {
        _medicines = medicines;
        _update = update;
    }

    public async Task<int> ExecuteAsync(IReadOnlyDictionary<Guid, bool> wanted, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(wanted);
        var changed = 0;
        foreach (var (id, email) in wanted)
        {
            var medicine = await _medicines.GetAsync(id, cancellationToken);
            if (medicine is null) continue;
            var channels = email
                ? medicine.NotificationChannels | NotificationChannels.Email
                : medicine.NotificationChannels & ~NotificationChannels.Email;
            if (channels == medicine.NotificationChannels) continue;
            var stored = new UpdateMedicineCommand(
                MedicineId: medicine.Id,
                Name: medicine.Name,
                ActiveIngredient: medicine.ActiveIngredient,
                Package: medicine.Package,
                Unit: medicine.Unit,
                ThresholdDays: medicine.ThresholdDays,
                NotificationChannels: medicine.NotificationChannels,
                EndDate: medicine.EndDate,
                DoctorName: medicine.DoctorName,
                Notes: medicine.Notes,
                IsActive: medicine.IsActive,
                RemindOnDose: medicine.RemindOnDose);
            await _update.ExecuteAsync(stored with { NotificationChannels = channels, Baseline = stored },
                cancellationToken);
            changed++;
        }
        return changed;
    }

    // Whether a medicine sends the low-stock email.
    public static bool IsOn(NotificationChannels channels) => (channels & NotificationChannels.Email) != 0;
}
