using OrganizeApp.Domain.Exceptions;

namespace OrganizeApp.Domain.Entities;

public partial class Category
{
    /// <summary>
    /// Constructor of Category
    /// </summary>
    /// <param name="name"></param>
    /// <param name="code"></param>
    /// <param name="description"></param>
    /// <param name="imageUrl"></param>
    /// <exception cref="CategoryDomainException"></exception>
    public Category(
        string name,
        string code,
        string description,
        string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CategoryDomainException("Category name is required.");

        if (string.IsNullOrWhiteSpace(Code))
            throw new CategoryDomainException("Category Code is required.");

        Name = name;
        Code = code;
        Description = description;
        ImageUrl = imageUrl;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CategoryDomainException("Category name is required.");

        Name = name;
    }

    public void ChangeCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new CategoryDomainException("Category name is required.");

        Code = code;
    }

    public void ChangeDescription(string description)
    {
        Description = description;
    }

    public void ChangeImage(string imageUrl)
    {
        ImageUrl = imageUrl;
    }
}