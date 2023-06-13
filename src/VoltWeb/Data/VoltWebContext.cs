using Microsoft.EntityFrameworkCore;
using VoltWeb.Models;

namespace VoltWeb.Data;

public class VoltWebContext : DbContext
{
    public VoltWebContext(DbContextOptions<VoltWebContext> options) : base(options)
    {
    }
    
    public DbSet<Hero> Hero { get; set; }

    public DbSet<Page> Page { get; set; }
    
    public DbSet<Blog> Blog { get; set; }
    
    public DbSet<BlogAuthor> BlogAuthors { get; set; }
}