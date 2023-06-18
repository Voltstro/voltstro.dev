using Microsoft.AspNetCore.Components;

namespace VoltWeb.Data;

/// <summary>
///     Contains all info on a blog post
/// </summary>
public struct BlogPostData
{
    /// <summary>
    ///     Title of the blog post
    /// </summary>
    public string Title { get; init; }
    
    /// <summary>
    ///     How long est should this post take to read
    /// </summary>
    public int ReadingTime { get; init; }
    
    /// <summary>
    ///     When was the blog post published
    /// </summary>
    public DateTime PublishedDate { get; init; }

    /// <summary>
    ///     Info the hero
    /// </summary>
    public HeroData HeroData { get; init; }
    
    /// <summary>
    ///     HTML content of the blog post
    /// </summary>
    public MarkupString HtmlContent { get; init; }
    
    /// <summary>
    ///     Does the blog post contain code blocks?
    /// </summary>
    public bool ContainsCodeBlocks { get; init; }
    
    /// <summary>
    ///     All headings contained in the blog post
    /// </summary>
    public string[] Headings { get; init; }

    /// <summary>
    ///     Who wrote this blog post?
    /// </summary>
    public Author[] Authors { get; init; }
    
    public struct Author
    {
        public string Name { get; init; }
        
        public string ImageBytes { get; init; }
    }
}