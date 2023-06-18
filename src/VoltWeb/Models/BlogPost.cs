using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VoltWeb.Models;

/// <summary>
///     DB Model for a blog post
/// </summary>
[Index(nameof(PostShortId), IsUnique = true)]
public class BlogPost
{
    /// <summary>
    ///     Primary Key
    /// </summary>
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    ///     Post short ID
    /// </summary>
    [Required]
    public string PostShortId { get; set; }
    
    /// <summary>
    ///     When was this blog post published
    ///
    ///     Null if the blog post should not be published
    /// </summary>
    public DateTime? PublishedDate { get; set; }
    
    /// <summary>
    ///     When was this blog post edited
    /// </summary>
    public DateTime? EditedDate { get; set; }
    
    /// <summary>
    ///     Display title
    /// </summary>
    [Required]
    public string Title { get; set; }
    
    /// <summary>
    ///     Hero
    /// </summary>
    [ForeignKey("Hero")]
    public int HeroId { get; set; }
    public virtual Hero Hero { get; set; }
    
    /// <summary>
    ///     Markdown content
    /// </summary>
    [Required]
    public string Content { get; set; }
}