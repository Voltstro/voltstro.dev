namespace VoltWeb.Data;

public struct MarkdownResult
{
    public string HtmlContent { get; init; }
    
    public bool ContainsCodeBlocks { get; init; }
    
    public string[] Headings { get; init; }
}