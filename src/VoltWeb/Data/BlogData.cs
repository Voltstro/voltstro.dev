namespace VoltWeb.Data;

/// <summary>
///     Basic data on a blog
/// </summary>
public struct BlogData
{
    public string Title { get; set; }
    
    public string Id { get; set; }
    
    public DateTime PublishedDate { get; set; }
}