using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizeApp.Application.DTOs;

public class NewCategoryDTO
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } = DateTime.Now;


}
