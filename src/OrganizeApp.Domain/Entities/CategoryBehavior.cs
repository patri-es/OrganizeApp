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
    /// <exception cref="CategoryDomainExceptions"></exception>
    public Category(
        string name,
        string code,
        string? description,
        string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CategoryDomainExceptions("Category name is required.");

        if (string.IsNullOrWhiteSpace(Code))
            throw new CategoryDomainExceptions("Category Code is required.");

        Name = name;
        Code = code;
        Description = description;
        ImageUrl = imageUrl;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new CategoryDomainExceptions("Category name is required.");

        Name = name;
    }

    public void ChangeCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new CategoryDomainExceptions("Category name is required.");

        Code = code;
    }

    public void ChangeDescription(string? description)
    {
        Description = description;
    }

    public void ChangeImage(string? imageUrl)
    {
        //if (string.IsNullOrWhiteSpace(imageUrl))
        //    throw new CategoryDomainExceptions("Category image is required.");

        ImageUrl = imageUrl;
    }

    //public void IncreaseStock(int quantity)
    //{
    //    if (quantity <= 0)
    //        throw new CategoryDomainExceptions(
    //            "The quantity to increase must be greater than zero.");

    //    Stock += quantity;
    //}

    //public void DecreaseStock(int quantity)
    //{
    //    if (quantity <= 0)
    //        throw new CategoryDomainExceptions(
    //            "The quantity to decrease must be greater than zero.");

    //    if (quantity > Stock)
    //        throw new CategoryDomainExceptions(
    //            "Stock cannot be reduced below zero.");

    //    Stock -= quantity;

    //    if (Stock == 0)
    //        IsAvailable = false;
    //}

    //public void Activate()
    //{
    //    if (Stock == 0)
    //        throw new CategoryDomainExceptions(
    //            "A product cannot be activated without stock.");

    //    if (Category is null)
    //        throw new CategoryDomainExceptions(
    //            "A product cannot be activated without a category.");

    //    IsAvailable = true;
    //}

    //public void Deactivate()
    //{
    //    IsAvailable = false;
    //}

    //public void AssignCategory(Category category)
    //{
    //    Category = category;
    //}

    //public void RemoveCategory()
    //{
    //    Category = null;
    //    IsAvailable = false;
    //}
}