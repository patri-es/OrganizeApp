using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizeApp.Application.Products.DTOs;

public class NewProductDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } = DateTime.Now;


}
