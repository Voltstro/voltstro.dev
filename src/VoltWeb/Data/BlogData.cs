using Microsoft.AspNetCore.Components;

namespace VoltWeb.Data;

public struct BlogData
{
    public string Title { get; init; }
    
    public int ReadingTime { get; init; }
    
    public DateTime PublishedDate { get; init; }

    public HeroData HeroData { get; init; }
    
    public MarkupString HtmlContent { get; init; }
    
    public bool ContainsCodeBlocks { get; init; }
    
    public string[] Headings { get; init; }

    public Author[] Authors { get; init; }
    
    public struct Author
    {
        public string Name { get; init; }
        
        public string ImageBytes { get; init; }
    }
}