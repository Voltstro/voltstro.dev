using System.ComponentModel.DataAnnotations;

namespace VoltWeb.Models;

public class Hero
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Text { get; set; }
}