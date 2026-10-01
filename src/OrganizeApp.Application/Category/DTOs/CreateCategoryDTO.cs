namespace OrganizeApp.Application.DTOs;

public class CreateCategoryDTO
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string ImageUrl { get; private set; } = string.Empty;
}
