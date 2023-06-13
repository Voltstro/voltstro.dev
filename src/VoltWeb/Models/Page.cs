using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltWeb.Models;

public class Page
{
    [Key]
    public string Id { get; set; }
    
    [ForeignKey("Hero")]
    public int HeroId { get; set; }
    public virtual Hero Hero { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Content { get; set; }
}