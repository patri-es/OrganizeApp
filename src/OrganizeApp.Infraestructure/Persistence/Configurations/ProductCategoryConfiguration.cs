using Microsoft.EntityFrameworkCore;
using OrganizeApp.Domain.Entities;

namespace OrganizeApp.Infraestructure.Persistence.Configurations;

public class ProductCategoryConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Products)
            .UsingEntity<Dictionary<string, object>>(
                "ProductCategories",
                right => right
                    .HasOne<Category>()
                    .WithMany()
                    .HasForeignKey("CategoryId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("ProductId", "CategoryId");
                    join.ToTable("ProductCategories");
                });
    }
}