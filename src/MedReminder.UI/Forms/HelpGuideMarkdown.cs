using System.Text.RegularExpressions;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace MedReminder.UI.Forms;

// Markdown → HTML for the integrated guide (HelpViewerForm).
//
// The guide is shipped content, but the copy under <bin>/localization
// is read first and the per-user install folder is writable by the
// user, so raw HTML in it is not trusted. The only raw HTML the guides
// use is the section anchor <a id="..."></a> targeted by the table of
// contents: those tags are kept, every other HTML block or tag is
// dropped before rendering. Markdig's DisableHtml() would escape the
// anchors too and break the table of contents.
internal static partial class HelpGuideMarkdown
{
    private static readonly MarkdownPipeline Pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions() // tables, task lists, autolinks, footnotes, etc.
        .Build();

    public static string ToHtml(string markdown)
    {
        ArgumentNullException.ThrowIfNull(markdown);
        var document = Markdown.Parse(markdown, Pipeline);
        foreach (var block in document.Descendants<HtmlBlock>().ToList())
        {
            block.Parent?.Remove(block);
        }
        foreach (var inline in document.Descendants<HtmlInline>().ToList())
        {
            if (!AnchorTag().IsMatch(inline.Tag)) inline.Remove();
        }
        return document.ToHtml(Pipeline);
    }

    [GeneratedRegex("^(<a id=\"[A-Za-z0-9-]+\">|</a>)$")]
    private static partial Regex AnchorTag();
}
