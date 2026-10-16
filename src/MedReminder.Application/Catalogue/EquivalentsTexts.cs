using System.Globalization;
using MedReminder.Application.Abstractions;

namespace MedReminder.Application.Catalogue;

// Localized wording of an equivalents view
// (ANALYSIS-IT-EQUIVALENTS-AND-INFO-LINK §2.6), shared by the desktop
// dialog and the Android app (backlog B4-05): the state when there is no
// group, else the group, the reference price, the packages already at
// home (with the AIFA note that can restrict the substitution) and the
// source with its list date. Factual: it reports the AIFA list and never
// suggests a substitution, which the disclaimer leaves to the doctor and
// the pharmacist.
public static class EquivalentsTexts
{
    public static string Header(EquivalentsView view, ILocalizationService loc)
    {
        ArgumentNullException.ThrowIfNull(view);
        ArgumentNullException.ThrowIfNull(loc);
        var c = loc.CurrentCulture;
        switch (view.State)
        {
            case EquivalentsState.NoCode:
                return loc.Get("Ui.EquivalentsDialog.NoCode");
            case EquivalentsState.NoList:
                return loc.Get("Ui.EquivalentsDialog.NoList");
            case EquivalentsState.NotListed:
                return loc.Get("Ui.EquivalentsDialog.NotListed", view.ListDate!.Value.ToString("d", c));
        }

        var group = view.Group!;
        var lines = new List<string>
        {
            loc.Get("Ui.EquivalentsDialog.Group", group.ActiveIngredient, group.Reference),
            loc.Get("Ui.EquivalentsDialog.ReferencePrice", group.ReferencePrice is { } reference ? Price(reference, c) : "—"),
        };
        // The packages of the group already in stock. A note on either
        // package can restrict the substitution: it is shown instead of a
        // plain "equivalent". The medicine's own package, in stock in
        // another medicine record, needs no note.
        var currentNote = view.Current?.Package.Note;
        foreach (var home in view.AtHome)
        {
            var names = string.Join(", ", home.AtHome);
            var notes = home.IsCurrent
                ? []
                : new[] { home.Package.Note, currentNote }
                    .OfType<string>()
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Distinct()
                    .ToList();
            lines.Add(notes.Count == 0
                ? loc.Get("Ui.EquivalentsDialog.AtHome", names, home.Package.Name)
                : loc.Get("Ui.EquivalentsDialog.AtHome.Note", names, home.Package.Name, string.Join(" / ", notes)));
        }
        lines.Add(loc.Get("Ui.EquivalentsDialog.Source", view.ListDate!.Value.ToString("d", c)));
        return string.Join(Environment.NewLine, lines);
    }

    // Euros in the user's number format.
    public static string Price(decimal euros, CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(culture);
        return string.Format(culture, "{0:0.00} €", euros);
    }
}
