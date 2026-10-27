namespace OrganizeApp.Domain.Entities;

public partial class Category
{
    public int Id { get; private set; }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; } 

    public string? ImageUrl { get; private set; } 
    public DateTime CreationDate { get; private set; }

    public ICollection<Product> Products { get; private set; } = [];
}