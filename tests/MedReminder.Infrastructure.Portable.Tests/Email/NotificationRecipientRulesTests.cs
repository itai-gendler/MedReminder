using FluentAssertions;
using MedReminder.Infrastructure.Email;
using Xunit;

namespace MedReminder.Infrastructure.Tests.Email;

// Android backlog B5-02: the checks of the recipients of a profile's
// email, shared by the desktop's Settings → Notifications and the phone.
public class NotificationRecipientRulesTests
{
    [Fact]
    public void Empty_or_full_mailboxes_have_no_problem()
    {
        NotificationRecipientRules.Check("", "", "").Should().BeEmpty();
        NotificationRecipientRules.Check(null, null, null).Should().BeEmpty();
        NotificationRecipientRules.Check(" me@example.org ", "Anna <anna@example.org>", "dr@example.org")
            .Should().BeEmpty();
    }

    [Fact]
    public void Each_address_must_be_a_mailbox_with_a_domain()
        => NotificationRecipientRules.Check("me", "anna@", "not an address").Should().Equal(
            NotificationRecipientProblem.RecipientInvalid,
            NotificationRecipientProblem.CaregiverInvalid,
            NotificationRecipientProblem.DoctorInvalid);

    [Fact]
    public void The_caregiver_is_not_the_recipient()
        => NotificationRecipientRules.Check("me@example.org", " ME@example.org", "")
            .Should().Equal(NotificationRecipientProblem.CaregiverSameAsRecipient);

    [Fact]
    public void A_caregiver_without_a_recipient_is_allowed()
        => NotificationRecipientRules.Check("", "anna@example.org", "").Should().BeEmpty();
}
