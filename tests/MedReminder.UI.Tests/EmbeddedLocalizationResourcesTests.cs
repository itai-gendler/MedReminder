using FluentAssertions;
using MedReminder.UI.Forms;
using Xunit;

namespace MedReminder.UI.Tests;

// The embedded guides and dictionaries are the fallback when the copies
// under <bin>/localization are missing. They must be in the main
// assembly: a .<lang>. suffix without WithCulture=false in the csproj
// moves them into satellite assemblies, which the fallback never loads.
public sealed class EmbeddedLocalizationResourcesTests
{
    public static TheoryData<string> Languages => new() { "en", "it", "fr", "es", "de" };

    [Theory]
    [MemberData(nameof(Languages))]
    public void Guide_and_dictionary_are_embedded_in_the_main_assembly(string language)
    {
        var names = typeof(HelpGuideMarkdown).Assembly.GetManifestResourceNames();

        names.Should().Contain($"USER_GUIDE.{language}.md");
        names.Should().Contain($"strings.{language}.json");
    }
}
