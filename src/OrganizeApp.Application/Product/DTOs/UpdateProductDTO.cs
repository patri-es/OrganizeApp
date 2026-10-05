using OrganizeApp.Application.Category.DTOs;

namespace OrganizeApp.Application.Product.DTOs;

public class UpdateProductDTO
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
}