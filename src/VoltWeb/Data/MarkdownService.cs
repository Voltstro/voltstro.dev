using Markdig;
using Markdig.Syntax;

namespace VoltWeb.Data;

public sealed class MarkdownService
{
    private readonly MarkdownPipeline pipeline;
    
    public MarkdownService()
    {
        pipeline = new MarkdownPipelineBuilder()
            .UseBootstrap()
            .UseAdvancedExtensions()
            .Build();
    }

    public MarkdownResult Parse(string markdown)
    {
        MarkdownDocument document = Markdown.Parse(markdown, pipeline);

        bool containsCodeBlocks = document.Any(x => x is FencedCodeBlock);
        
        //Headers
        string[] headers = document
            .Select(x => x as HeadingBlock)
            .Where(x => x is { Level: 2 })
            .Select(hb => hb.Inline.FirstChild.ToString())
            .ToArray();

        return new MarkdownResult()
        {
            ContainsCodeBlocks = containsCodeBlocks,
            HtmlContent = document.ToHtml(pipeline),
            Headings = headers
        };
    }
}