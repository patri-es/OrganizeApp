
namespace OrganizeApp.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Name { get; set; } 
    public string? Description { get; set; }
}
