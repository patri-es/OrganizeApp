using OrganizeApp.Application.Category.DTOs;

namespace OrganizeApp.Application.Product.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int Stock { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Price { get; set; }
    public List<CategoryDTO> Categories { get; set; } = [];
}