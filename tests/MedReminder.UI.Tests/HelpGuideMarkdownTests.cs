using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using MedReminder.UI.Forms;
using Xunit;

namespace MedReminder.UI.Tests;

// The guide keeps only the section anchors as raw HTML (HelpGuideMarkdown).
public sealed class HelpGuideMarkdownTests
{
    public static TheoryData<string> Languages => new() { "en", "it", "fr", "es", "de" };

    [Fact]
    public void Section_anchors_are_kept()
    {
        var html = HelpGuideMarkdown.ToHtml("<a id=\"start\"></a>\n## Getting started\n\nSee [start](#start).");

        html.Should().Contain("<a id=\"start\"></a>");
        html.Should().Contain("<a href=\"#start\">start</a>");
    }

    [Theory]
    [InlineData("<script>alert(1)</script>")]
    [InlineData("<div onclick=\"alert(1)\">x</div>")]
    [InlineData("<iframe src=\"https://example.com\"></iframe>")]
    [InlineData("<meta http-equiv=\"refresh\" content=\"0;url=https://example.com\">")]
    public void Html_blocks_are_dropped(string block)
    {
        var html = HelpGuideMarkdown.ToHtml("Text.\n\n" + block + "\n\nMore text.");

        html.Should().NotContainAny("<script", "<div", "<iframe", "<meta");
        html.Should().Contain("More text.");
    }

    [Theory]
    [InlineData("<img src=x onerror=\"alert(1)\">")]
    [InlineData("<a href=\"javascript:alert(1)\">")]
    [InlineData("<a id=\"x\" onclick=\"alert(1)\">")]
    [InlineData("<span style=\"display:none\">")]
    public void Inline_html_other_than_anchors_is_dropped(string tag)
    {
        var html = HelpGuideMarkdown.ToHtml("Before " + tag + "after.");

        html.Should().NotContainAny("<img", "javascript:", "onclick", "<span");
        html.Should().Contain("Before after.");
    }

    // Every anchor of every shipped guide survives, so the table of
    // contents keeps working, and none is shown as escaped text.
    [Theory]
    [MemberData(nameof(Languages))]
    public void Shipped_guides_keep_every_section_anchor(string language)
    {
        var markdown = ReadEmbeddedGuide(language);
        var anchors = Regex.Count(markdown, "<a id=\"[A-Za-z0-9-]+\"></a>");

        var html = HelpGuideMarkdown.ToHtml(markdown);

        anchors.Should().BePositive();
        Regex.Count(html, "<a id=\"[A-Za-z0-9-]+\"></a>").Should().Be(anchors);
        html.Should().NotContain("&lt;a id=");
    }

    private static string ReadEmbeddedGuide(string language)
    {
        using var stream = typeof(HelpGuideMarkdown).Assembly
            .GetManifestResourceStream($"USER_GUIDE.{language}.md")!;
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd();
    }
}
