using System.Text.RegularExpressions;
using MedReminder.Application.UseCases;
using MedReminder.Domain.Catalogue;

namespace MedReminder.Application.Catalogue;

// What a medicine form takes from a picked catalogue entry, shared by the
// desktop form (autocomplete pick and barcode scan) and the phone (Android
// backlog B4-03), so a medicine linked on either device gets the same
// name, active ingredient, package and unit:
//   - Name: the commercial name extended with the strength and the
//     pharmaceutical form when the row has them ("TACHIPIRINA 500 MG
//     compresse"), so two strengths of one product stay distinct;
//   - ActiveIngredient: the reference ingredients, " / "-joined;
//   - Package: the row's dosage description (AIFA DESCRIZIONE), what the
//     box says, or null when the row has none (the form keeps its value);
//   - Unit: the unit dictionary key for the pharmaceutical form when the
//     form is known, the raw form otherwise, null without a form (the
//     form keeps its value);
//   - Link: national code, the first ATC code of the ingredients and the
//     row id, for AddMedicine / UpdateMedicine.
public static class CatalogueEntryFill
{
    // Extracts the first strength token ("500 MG COMPRESSE 20 …" →
    // "500 MG"): a number followed by a common pharmaceutical unit.
    // Conservative: no match leaves the name without a strength.
    private static readonly Regex StrengthRegex = new(
        @"\b\d+([.,]\d+)?\s*(mg|g|mcg|µg|ug|ml|l|ui|iu|%)\b",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    public static string Name(ReferenceMedicine reference)
    {
        ArgumentNullException.ThrowIfNull(reference);
        var parts = new List<string>(3) { reference.CommercialName };
        if (StrengthOf(reference.Dosage) is { } strength)
        {
            parts.Add(strength);
        }
        if (!string.IsNullOrWhiteSpace(reference.PharmaceuticalForm))
        {
            parts.Add(reference.PharmaceuticalForm.Trim().ToLowerInvariant());
        }
        return string.Join(" ", parts);
    }

    public static string ActiveIngredient(ReferenceMedicine reference)
    {
        ArgumentNullException.ThrowIfNull(reference);
        return string.Join(" / ", reference.ActiveIngredients.Select(a => a.Name));
    }

    public static string? Package(ReferenceMedicine reference)
    {
        ArgumentNullException.ThrowIfNull(reference);
        return string.IsNullOrWhiteSpace(reference.Dosage) ? null : reference.Dosage;
    }

    // The unit for the form: `localize` turns a unit dictionary key
    // (Ui.MedicineEditDialog.Unit.*) into the label of the app language.
    // AIFA FORMA is free text with many variants: matched by contains on
    // lowercased Italian stems; an unknown form is kept as it is.
    public static string? Unit(string? pharmaceuticalForm, Func<string, string> localize)
    {
        ArgumentNullException.ThrowIfNull(localize);
        if (string.IsNullOrWhiteSpace(pharmaceuticalForm)) return null;
        return UnitKey(pharmaceuticalForm) is { } key ? localize(key) : pharmaceuticalForm.Trim();
    }

    // The unit dictionary key for a pharmaceutical form, or null when the
    // form is not one of the known stems.
    public static string? UnitKey(string? pharmaceuticalForm)
    {
        if (string.IsNullOrWhiteSpace(pharmaceuticalForm)) return null;
        var lower = pharmaceuticalForm.ToLowerInvariant();

        bool Has(string needle) => lower.Contains(needle, StringComparison.Ordinal);

        if (Has("compress")) return "Ui.MedicineEditDialog.Unit.Tablets";
        if (Has("capsul")) return "Ui.MedicineEditDialog.Unit.Capsules";
        if (Has("bustin")) return "Ui.MedicineEditDialog.Unit.Sachets";
        if (Has("granulat")) return "Ui.MedicineEditDialog.Unit.Granules";
        if (Has("gocce") || Has("collir")) return "Ui.MedicineEditDialog.Unit.Drops";
        if (Has("sciropp") || Has("sospension") || Has("soluzion") || Has("sospens")) return "Ui.MedicineEditDialog.Unit.Ml";
        if (Has("supposte") || Has("supposta")) return "Ui.MedicineEditDialog.Unit.Suppositories";
        if (Has("cerott")) return "Ui.MedicineEditDialog.Unit.Patches";
        if (Has("crema") || Has("unguent") || Has("pomata") || Has("gel")) return "Ui.MedicineEditDialog.Unit.Grams";
        if (Has("spray") || Has("erogazion") || Has("polvere per inalazion")) return "Ui.MedicineEditDialog.Unit.Puffs";
        if (Has("fiala") || Has("fiale")) return "Ui.MedicineEditDialog.Unit.Ampoules";
        if (Has("flacon")) return "Ui.MedicineEditDialog.Unit.Vials";
        if (Has("polvere")) return "Ui.MedicineEditDialog.Unit.Sachets";
        return null;
    }

    public static CatalogueLink Link(ReferenceMedicine reference)
    {
        ArgumentNullException.ThrowIfNull(reference);
        var atc = reference.ActiveIngredients.Select(a => a.Atc).FirstOrDefault(a => a.HasValue);
        return new CatalogueLink(reference.NationalCode, atc, reference.Id);
    }

    public static string? StrengthOf(string? dosage)
    {
        if (string.IsNullOrWhiteSpace(dosage)) return null;
        var match = StrengthRegex.Match(dosage);
        return match.Success ? match.Value : null;
    }
}
