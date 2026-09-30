
namespace OrganizeApp.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } 
    public string? Image { get; set; } 
    public string? Category { get; set; } 
    public int Price { get; set; } = 0;
    public int Stock { get; set; } = 0;

}
