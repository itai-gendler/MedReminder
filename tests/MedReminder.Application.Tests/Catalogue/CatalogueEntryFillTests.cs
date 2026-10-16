using FluentAssertions;
using MedReminder.Application.Catalogue;
using MedReminder.Domain.Catalogue;
using Xunit;

namespace MedReminder.Application.Tests.Catalogue;

// The values a medicine form takes from a picked catalogue entry, shared
// by the desktop form and the Android app (backlog B4-03).
public class CatalogueEntryFillTests
{
    private static ReferenceMedicine Row(
        string name = "TACHIPIRINA", string? dosage = "500 MG COMPRESSE 20 COMPRESSE IN BLISTER PVC/AL",
        string? form = "COMPRESSA", params (string Name, string? Atc)[] ingredients)
        => new()
        {
            Country = CountryCode.Parse("IT"),
            NationalCode = "012745093",
            CommercialName = name,
            Dosage = dosage,
            PharmaceuticalForm = form,
            SnapshotVersion = "202610",
            ActiveIngredients = [.. ingredients.Select(i => new ReferenceActiveIngredient
            {
                Country = CountryCode.Parse("IT"),
                Name = i.Name,
                Atc = i.Atc is null ? null : AtcCode.Parse(i.Atc),
            })],
        };

    [Fact]
    public void Name_adds_strength_and_form()
        => CatalogueEntryFill.Name(Row()).Should().Be("TACHIPIRINA 500 MG compressa");

    [Fact]
    public void Name_without_strength_or_form_is_the_commercial_name()
        => CatalogueEntryFill.Name(Row(dosage: "20 BUSTINE", form: null)).Should().Be("TACHIPIRINA");

    [Theory]
    [InlineData("COMPRESSA RIVESTITA", "Ui.MedicineEditDialog.Unit.Tablets")]
    [InlineData("capsula rigida", "Ui.MedicineEditDialog.Unit.Capsules")]
    [InlineData("GOCCE ORALI, SOLUZIONE", "Ui.MedicineEditDialog.Unit.Drops")]
    [InlineData("SCIROPPO", "Ui.MedicineEditDialog.Unit.Ml")]
    [InlineData("POLVERE PER INALAZIONE", "Ui.MedicineEditDialog.Unit.Puffs")]
    [InlineData("Film-coated tablet", null)]
    [InlineData(null, null)]
    public void UnitKey_maps_known_forms(string? form, string? key)
        => CatalogueEntryFill.UnitKey(form).Should().Be(key);

    [Fact]
    public void Unit_localizes_a_known_form_and_keeps_an_unknown_one()
    {
        CatalogueEntryFill.Unit("COMPRESSA", key => "[" + key + "]").Should().Be("[Ui.MedicineEditDialog.Unit.Tablets]");
        CatalogueEntryFill.Unit(" Film-coated tablet ", key => key).Should().Be("Film-coated tablet");
        CatalogueEntryFill.Unit(" ", key => key).Should().BeNull();
    }

    [Fact]
    public void Ingredients_package_and_link()
    {
        var row = Row(ingredients: [("PARACETAMOLO", null), ("CODEINA", "N02AJ06")]);

        CatalogueEntryFill.ActiveIngredient(row).Should().Be("PARACETAMOLO / CODEINA");
        CatalogueEntryFill.Package(row).Should().Be("500 MG COMPRESSE 20 COMPRESSE IN BLISTER PVC/AL");
        CatalogueEntryFill.Package(Row(dosage: " ")).Should().BeNull();
        var link = CatalogueEntryFill.Link(row);
        link.NationalCode.Should().Be("012745093");
        link.AtcCode.Should().Be(AtcCode.Parse("N02AJ06"));
        link.LinkedReferenceMedicineId.Should().Be(row.Id);
    }
}
