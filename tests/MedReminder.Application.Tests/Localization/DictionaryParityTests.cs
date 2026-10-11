using System.Text.Json;
using FluentAssertions;
using MedReminder.Application.Abstractions;
using Xunit;

namespace MedReminder.Application.Tests.Localization;

// Every localized dictionary (it, fr, es, de) must define exactly
// the same set of keys as the reference English dictionary — no
// missing keys, no extras. Prevents drift as new UI strings are
// added. A key defined twice in one file is rejected too: the parser
// keeps the last value and silently drops the other.
public class DictionaryParityTests
{
    [Theory]
    [InlineData("it")]
    [InlineData("fr")]
    [InlineData("es")]
    [InlineData("de")]
    public void Localized_dictionary_covers_every_english_key(string languageCode)
    {
        var reference = LoadDictionary(SupportedLanguages.Default);
        var localized = LoadDictionary(languageCode);

        var missing = reference.Keys.Except(localized.Keys).OrderBy(k => k).ToArray();
        var extra = localized.Keys.Except(reference.Keys).OrderBy(k => k).ToArray();

        missing.Should().BeEmpty($"'{languageCode}' is missing translations for these English keys");
        extra.Should().BeEmpty($"'{languageCode}' defines keys absent from the English reference");
    }

    [Theory]
    [InlineData("en")]
    [InlineData("it")]
    [InlineData("fr")]
    [InlineData("es")]
    [InlineData("de")]
    public void Dictionary_defines_each_key_once(string languageCode)
    {
        using var document = JsonDocument.Parse(File.ReadAllText(DictionaryPath(languageCode)));
        var duplicates = document.RootElement.EnumerateObject()
            .GroupBy(p => p.Name, StringComparer.Ordinal)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .OrderBy(k => k)
            .ToArray();

        duplicates.Should().BeEmpty($"'{languageCode}' defines these keys more than once");
    }

    private static string DictionaryPath(string languageCode)
        => Path.Combine(AppContext.BaseDirectory, "localization", $"strings.{languageCode}.json");

    private static Dictionary<string, string> LoadDictionary(string languageCode)
    {
        var path = DictionaryPath(languageCode);
        File.Exists(path).Should().BeTrue($"expected dictionary at {path}");
        using var stream = File.OpenRead(path);
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(stream);
        dict.Should().NotBeNull();
        return dict!;
    }
}
