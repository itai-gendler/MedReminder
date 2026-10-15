using MedReminder.Application.Notifications;
using MedReminder.Domain.Prescriptions;

namespace MedReminder.Application.Prescriptions;

// The repeatable prescription a low-stock notice names (NotificationTexts)
// and offers to open instead of preparing a new request
// (NotificationTarget.HasRepeatablePrescription). Shared by the desktop
// pass (MedicationMonitor) and the device plan (NotificationPlanner), so
// both choose the same prescription and the same action. Pure.
public static class RepeatablePrescriptionNotices
{
    // Among the medicine's prescriptions, the repeatable one to collect on
    // `day` with dispensations left: the one that lapses first (no end
    // date last). `collected` holds the dispensations recorded per
    // prescription id (IPrescriptionDispensationRepository
    // .CountByPrescriptionAsync). Null when there is none.
    public static RepeatablePrescriptionNotice? Select(
        IEnumerable<Prescription> prescriptions,
        IReadOnlyDictionary<Guid, int> collected,
        DateOnly day)
    {
        ArgumentNullException.ThrowIfNull(prescriptions);
        ArgumentNullException.ThrowIfNull(collected);
        var open = prescriptions
            .Where(p => p.IsRepeatable)
            .Select(p => (Prescription: p, Count: collected.GetValueOrDefault(p.Id)))
            .Where(x => x.Prescription.StatusOn(day, x.Count) == PrescriptionStatus.ToCollect)
            .OrderBy(x => x.Prescription.ValidUntil ?? DateOnly.MaxValue)
            .FirstOrDefault();
        return open.Prescription is null
            ? null
            : new RepeatablePrescriptionNotice(
                PrescriptionRules.DispensationsLeft(open.Prescription, open.Count), open.Prescription.ValidUntil);
    }
}
