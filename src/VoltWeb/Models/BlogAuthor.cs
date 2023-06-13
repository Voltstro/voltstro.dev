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
    public string BlogId { get; set; }
    public virtual Blog Blog { get; set; }
}