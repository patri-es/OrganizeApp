using Microsoft.EntityFrameworkCore;
using OrganizeApp.Domain.Entities;

namespace OrganizeApp.Infraestructure.Persistence;

public class AppDbContext : DbContext
{
    // contructor
    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
    {

    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}
