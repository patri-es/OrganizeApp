namespace OrganizeApp.Domain.Entities;

public partial class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? ImageUrl { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public DateTime CreationDate { get; private set; }
    public bool IsAvailable { get; private set; }

    // N:N relationship
    public ICollection<Category> Categories { get; private set; } = [];
}