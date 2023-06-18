using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoltWeb.Models;

public class BlogAuthor
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string AzureId { get; set; }
    
    [ForeignKey("Blog")]
    public int BlogId { get; set; }
    public virtual BlogPost BlogPost { get; set; }
}