using FluentAssertions;
using MedReminder.Application.Notifications;
using MedReminder.Application.Tests.Support;
using MedReminder.Application.UseCases;
using MedReminder.Domain.Notifications;
using Xunit;

namespace MedReminder.Application.Tests.Notifications;

// Android backlog B5-02: the low-stock email of each medicine, turned on
// or off without touching the other channel or the other fields.
public class UpdateLowStockEmailTests
{
    private static Task<Guid> AddAsync(ApplicationTestScope scope, string name, NotificationChannels channels)
        => scope.AddMedicine.ExecuteAsync(
            new AddMedicineCommand(name, "compresse", 1m, 1, new DateOnly(2026, 9, 1), 7, channels,
                Notes: "  note  ", InitialQuantity: 30m),
            CancellationToken.None);

    private static UpdateLowStockEmail Use(ApplicationTestScope scope) => new(scope.Medicines, scope.UpdateMedicine);

    [Fact]
    public async Task Email_is_turned_on_and_off_keeping_the_device_notice()
    {
        var scope = new ApplicationTestScope();
        var notice = await AddAsync(scope, "Ramipril", NotificationChannels.Windows);
        var both = await AddAsync(scope, "Enalapril", NotificationChannels.Both);
        var email = await AddAsync(scope, "Bisoprolol", NotificationChannels.Email);

        var changed = await Use(scope).ExecuteAsync(
            new Dictionary<Guid, bool> { [notice] = true, [both] = false, [email] = false }, CancellationToken.None);

        changed.Should().Be(3);
        (await scope.Medicines.GetAsync(notice, CancellationToken.None))!.NotificationChannels
            .Should().Be(NotificationChannels.Both);
        (await scope.Medicines.GetAsync(both, CancellationToken.None))!.NotificationChannels
            .Should().Be(NotificationChannels.Windows);
        (await scope.Medicines.GetAsync(email, CancellationToken.None))!.NotificationChannels
            .Should().Be(NotificationChannels.None);
    }

    [Fact]
    public async Task A_medicine_already_as_wanted_or_deleted_is_skipped()
    {
        var scope = new ApplicationTestScope();
        var both = await AddAsync(scope, "Enalapril", NotificationChannels.Both);

        var changed = await Use(scope).ExecuteAsync(
            new Dictionary<Guid, bool> { [both] = true, [Guid.NewGuid()] = true }, CancellationToken.None);

        changed.Should().Be(0);
        (await scope.Medicines.GetAsync(both, CancellationToken.None))!.NotificationChannels
            .Should().Be(NotificationChannels.Both);
    }

    [Fact]
    public async Task Only_the_channels_are_written()
    {
        var scope = new ApplicationTestScope();
        var id = await AddAsync(scope, "Ramipril", NotificationChannels.Windows);
        // A value UpdateMedicine would trim if it were written.
        (await scope.Medicines.GetAsync(id, CancellationToken.None))!.Notes = "  note  ";

        await Use(scope).ExecuteAsync(new Dictionary<Guid, bool> { [id] = true }, CancellationToken.None);

        var after = await scope.Medicines.GetAsync(id, CancellationToken.None);
        after!.Notes.Should().Be("  note  ");
        after.ThresholdDays.Should().Be(7);
        UpdateLowStockEmail.IsOn(after.NotificationChannels).Should().BeTrue();
    }
}
