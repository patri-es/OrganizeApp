using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizeApp.Domain.Entities;


namespace OrganizeApp.Infraestructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Table
        builder.ToTable("Categories");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Name
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Code
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(20);

        // Description
        builder.Property(c => c.Description)
            .IsRequired(false)
            .HasMaxLength(500);

        // ImageUrl
        builder.Property(c => c.ImageUrl)
            .IsRequired(false)
            .HasMaxLength(500);

        // CreationDate
        builder.Property(c => c.CreationDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
    }
}
