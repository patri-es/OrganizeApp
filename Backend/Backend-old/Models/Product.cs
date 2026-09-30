using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public int Category { get; set; } = 0;
    public bool Available { get; set; } = false;
    public int Stock { get; set; } = 0;
}
