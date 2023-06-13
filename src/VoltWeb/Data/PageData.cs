using Microsoft.AspNetCore.Components;

namespace VoltWeb.Data;

public struct PageData
{
    public string Name { get; init; }
    
    public MarkupString HtmlContent { get; init; }
}