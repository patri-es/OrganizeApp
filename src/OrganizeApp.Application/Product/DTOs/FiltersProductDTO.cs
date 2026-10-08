namespace OrganizeApp.Application.Product.DTOs;

public class FiltersProductDTO
{
    public int? CategoryId { get; set; }
    public bool? HasCategory { get; set; }
    public bool? IsAvailable { get; set; }
    public int? MinStock { get; set; }
    public int? MaxStock { get; set; }
}