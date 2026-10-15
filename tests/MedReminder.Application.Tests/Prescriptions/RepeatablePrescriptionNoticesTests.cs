using FluentAssertions;
using MedReminder.Application.Notifications;
using MedReminder.Application.Prescriptions;
using MedReminder.Domain.Prescriptions;
using Xunit;

namespace MedReminder.Application.Tests.Prescriptions;

public sealed class RepeatablePrescriptionNoticesTests
{
    private static readonly DateOnly Today = new(2026, 10, 15);
    private static readonly Guid MedicineId = Guid.NewGuid();

    [Fact]
    public void Picks_the_repeatable_prescription_that_lapses_first()
    {
        var later = Repeatable(Today.AddDays(40), 3);
        var sooner = Repeatable(Today.AddDays(10), 2);
        var noEnd = Repeatable(null, 4);

        var notice = RepeatablePrescriptionNotices.Select([noEnd, later, sooner],
            new Dictionary<Guid, int> { [sooner.Id] = 1 }, Today);

        notice.Should().Be(new RepeatablePrescriptionNotice(1, Today.AddDays(10)));
    }

    [Fact]
    public void A_prescription_without_end_date_comes_last()
    {
        var noEnd = Repeatable(null, 4);

        RepeatablePrescriptionNotices.Select([noEnd], new Dictionary<Guid, int>(), Today)
            .Should().Be(new RepeatablePrescriptionNotice(4, null));
    }

    [Fact]
    public void Ignores_single_collected_lapsed_and_requested_prescriptions()
    {
        var single = new Prescription { MedicineId = MedicineId, IssuedOn = Today, ValidUntil = Today.AddDays(5) };
        var collected = Repeatable(Today.AddDays(30), 2);
        var lapsed = Repeatable(Today.AddDays(-1), 2);
        var requested = new Prescription { MedicineId = MedicineId, RequestedOn = Today, Dispensations = 3 };

        RepeatablePrescriptionNotices.Select([single, collected, lapsed, requested],
                new Dictionary<Guid, int> { [collected.Id] = 2 }, Today)
            .Should().BeNull();
    }

    [Fact]
    public void Is_evaluated_on_the_given_day()
    {
        var prescription = Repeatable(Today.AddDays(2), 3);
        var none = new Dictionary<Guid, int>();

        RepeatablePrescriptionNotices.Select([prescription], none, Today.AddDays(2)).Should().NotBeNull();
        RepeatablePrescriptionNotices.Select([prescription], none, Today.AddDays(3)).Should().BeNull();
    }

    private static Prescription Repeatable(DateOnly? validUntil, int dispensations) => new()
    {
        MedicineId = MedicineId,
        IssuedOn = Today.AddDays(-5),
        ValidUntil = validUntil,
        Dispensations = dispensations,
    };
}
