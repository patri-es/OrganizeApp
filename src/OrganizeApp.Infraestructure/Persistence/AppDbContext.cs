using Microsoft.EntityFrameworkCore;
using OrganizeApp.Domain.Entities;
using OrganizeApp.Infraestructure.Persistence.Configurations;

namespace OrganizeApp.Infraestructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    // ProductCategoryConfiguration  relationship refernce. Whit this ApplyConfigurationsFromAssembly() can find the relationship
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ProductCategoryConfiguration.Configure(modelBuilder);
    }
}