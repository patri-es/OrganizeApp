
namespace OrganizeApp.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } 
    public DateTime CreationDate { get; set; }

    public int? CategoryId { get; set; }
    public bool Available { get; set; } = true;
    public int Stock { get; set; } = 0;

}
