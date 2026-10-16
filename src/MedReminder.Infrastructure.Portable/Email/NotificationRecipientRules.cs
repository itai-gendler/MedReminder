namespace MedReminder.Infrastructure.Email;

// What a form checks before saving the recipients of a profile's email
// (Settings → Notifications on the desktop, the phone's recipients
// screen; Android backlog B5-02): each address that is set is one full
// mailbox the sender can parse (SmtpTransportRules.IsMailbox), and the
// caregiver is not the recipient (the sender would skip the copy). An
// empty address is allowed: no recipient, caregiver or doctor.
public enum NotificationRecipientProblem
{
    RecipientInvalid,
    CaregiverInvalid,
    CaregiverSameAsRecipient,
    DoctorInvalid,
}

public static class NotificationRecipientRules
{
    public static IReadOnlyList<NotificationRecipientProblem> Check(string? toAddress, string? caregiverAddress,
        string? doctorAddress)
    {
        var to = toAddress?.Trim() ?? string.Empty;
        var caregiver = caregiverAddress?.Trim() ?? string.Empty;
        var doctor = doctorAddress?.Trim() ?? string.Empty;
        var problems = new List<NotificationRecipientProblem>();
        if (to.Length > 0 && !SmtpTransportRules.IsMailbox(to)) problems.Add(NotificationRecipientProblem.RecipientInvalid);
        if (caregiver.Length > 0)
        {
            if (!SmtpTransportRules.IsMailbox(caregiver)) problems.Add(NotificationRecipientProblem.CaregiverInvalid);
            else if (string.Equals(caregiver, to, StringComparison.OrdinalIgnoreCase))
                problems.Add(NotificationRecipientProblem.CaregiverSameAsRecipient);
        }
        if (doctor.Length > 0 && !SmtpTransportRules.IsMailbox(doctor)) problems.Add(NotificationRecipientProblem.DoctorInvalid);
        return problems;
    }
}
