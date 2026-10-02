namespace OrganizeApp.Domain.Entities;

public partial class Product
{
    private static readonly List<Category> categories = [];

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public DateTime CreationDate { get; private set; }
    public bool IsAvailable { get; private set; }
    public ICollection<Category> Categories { get; private set; } = categories;
}