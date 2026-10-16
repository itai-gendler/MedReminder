using System.Globalization;
using FluentAssertions;
using MedReminder.Application.Catalogue;
using MedReminder.Application.Tests.Support;
using MedReminder.Domain.Catalogue;
using Xunit;

namespace MedReminder.Application.Tests.Catalogue;

// The equivalents wording shared by the desktop dialog and the Android
// app (backlog B4-05).
public class EquivalentsTextsTests
{
    private static readonly DateOnly ListDate = new(2026, 10, 1);
    private readonly JsonDictionaryLocalizationService _loc = new("en");

    [Fact]
    public void States_without_a_group_say_why()
    {
        EquivalentsTexts.Header(new EquivalentsView(EquivalentsState.NoCode, null, null, null, [], null), _loc)
            .Should().Contain("AIC");
        EquivalentsTexts.Header(new EquivalentsView(EquivalentsState.NoList, "012745093", null, null, [], null), _loc)
            .Should().Contain("not been downloaded");
        EquivalentsTexts.Header(new EquivalentsView(EquivalentsState.NotListed, "012745093", ListDate, null, [], null), _loc)
            .Should().Contain(ListDate.ToString("d", CultureInfo.GetCultureInfo("en")));
        _loc.FallbackHits.Should().BeEmpty();
    }

    [Fact]
    public void A_group_names_ingredient_reference_price_packages_at_home_with_notes_and_source()
    {
        var own = new EquivalentPackage("012745093", "TACHIPIRINA", "500 MG 20 CPR", "Angelini", 3.10m, 0.40m, null);
        var other = new EquivalentPackage("034567012", "PARACETAMOLO X", "500 MG 20 CPR", "X Pharma", 2.70m, 0m, "Not for children");
        var group = new EquivalenceGroup("G1", "PARACETAMOLO", "500 MG 20 UNITA'", "N02BE01", 2.70m, [own, other]);
        var view = new EquivalentsView(EquivalentsState.Listed, "012745093", ListDate, group,
        [
            new EquivalentRow(other, false, null, ["Paracetamol at home"]),
            new EquivalentRow(own, true, null, []),
        ], null);

        var text = EquivalentsTexts.Header(view, _loc);

        text.Should().Contain("PARACETAMOLO").And.Contain("2.70 €").And.Contain("Paracetamol at home")
            .And.Contain("Not for children").And.Contain(ListDate.ToString("d", CultureInfo.GetCultureInfo("en")));
        _loc.FallbackHits.Should().BeEmpty();
    }

    [Fact]
    public void Price_uses_the_culture()
        => EquivalentsTexts.Price(3.5m, CultureInfo.GetCultureInfo("it")).Should().Be("3,50 €");
}
