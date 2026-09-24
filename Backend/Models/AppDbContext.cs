using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class AppDbContext : DbContext
{
    // contructor
    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
    {

    }

    public DbSet<Person> People { get; set; }
    public DbSet<Product> Products { get; set; }
    //public DbSet<Category> Categories { get; set; }
}
