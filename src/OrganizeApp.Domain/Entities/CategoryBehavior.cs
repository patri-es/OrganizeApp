using OrganizeApp.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace OrganizeApp.Domain.Entities;

public partial class Category
{
    [GeneratedRegex(@"^[A-Z0-9_-]+$")]
    private static partial Regex CodeRegex();
    private const int MaxCodeLength = 20;
    private const int MaxNameLength = 100;
    private const int MaxDescriptionLength = 500;
    private const int MaxImageUrlLength = 500;


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

        if (string.IsNullOrWhiteSpace(code))
            throw new CategoryDomainException("Category Code is required.");

        // Code format
        if (!CodeRegex().IsMatch(code))
            throw new CategoryDomainException(
                "Category code can only contain uppercase letters, numbers, hyphens and underscores.");

        // maxLength
        if (code.Length > MaxCodeLength)
            throw new CategoryDomainException($"Category code cannot exceed {MaxCodeLength} characters.");

        if (name.Length > MaxNameLength)
            throw new CategoryDomainException($"Category name cannot exceed {MaxNameLength} characters.");

        if (description?.Length > MaxDescriptionLength)
            throw new CategoryDomainException($"Category description cannot exceed {MaxDescriptionLength} characters.");

        if (imageUrl?.Length > MaxImageUrlLength)
            throw new CategoryDomainException($"Category imageUrl cannot exceed {MaxImageUrlLength} characters.");


        Name = name;
        Code = code;
        Description = description;
        ImageUrl = imageUrl;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CategoryDomainException("Category name is required.");

        if (name.Length > MaxNameLength)
            throw new CategoryDomainException(
                $"Category name cannot exceed {MaxNameLength} characters.");

        Name = name;
    }

    public void ChangeCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new CategoryDomainException("Category code is required.");

        if (code.Length > MaxCodeLength)
            throw new CategoryDomainException(
                $"Category code cannot exceed {MaxCodeLength} characters.");

        if (!CodeRegex().IsMatch(code))
            throw new CategoryDomainException(
                "Category code can only contain uppercase letters, numbers, hyphens and underscores.");

        Code = code;
    }

    public void ChangeDescription(string description)
    {

        if (description.Length > MaxDescriptionLength)
            throw new CategoryDomainException(
                $"Category description cannot exceed {MaxDescriptionLength} characters.");

        Description = description;
    }

    public void ChangeImage(string imageUrl)
    {
        if (imageUrl.Length > MaxImageUrlLength)
            throw new CategoryDomainException(
                $"Category imageUrl cannot exceed {MaxImageUrlLength} characters.");

        ImageUrl = imageUrl;
    }

}