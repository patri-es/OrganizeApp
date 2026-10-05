using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizeApp.Application.Category.DTOs;

public class UpdateCategoryDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;
}
