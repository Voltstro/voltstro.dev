using Microsoft.AspNetCore.Components;

namespace VoltWeb.Data;

public struct BlogData
{
    public string Title { get; init; }
    
    public int ReadingTime { get; init; }
    
    public DateTime PublishedDate { get; init; }

    public HeroData HeroData { get; init; }
    
    public MarkupString HtmlContent { get; set; }
}