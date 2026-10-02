using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizeApp.Domain.Entities;

namespace OrganizeApp.Infraestructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Table
        builder.ToTable("Products");

        // Primary Key
        builder.HasKey(p => p.Id);

        // Name
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Description
        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasMaxLength(500);

        // ImageUrl
        builder.Property(p => p.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        // Price
        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        // Stock
        builder.Property(p => p.Stock)
            .IsRequired();

        // CreationDate
        builder.Property(p => p.CreationDate)
            .IsRequired();

        // IsAvailable
        builder.Property(p => p.IsAvailable)
            .IsRequired();
    }
}
