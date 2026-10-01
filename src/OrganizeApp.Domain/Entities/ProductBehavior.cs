using OrganizeApp.Domain.Exceptions;

namespace OrganizeApp.Domain.Entities;

public partial class Product
{
    /// <summary>
    /// Constructor of Category
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="price"></param>
    /// <param name="imageUrl"></param>
    /// <exception cref="ProductDomainException"></exception>
    public Product(
        string name,
        string? description,
        decimal price,
        string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ProductDomainException("Product name is required.");

        if (price < 0)
            throw new ProductDomainException("Product price cannot be negative.");

        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ProductDomainException("Product image is required.");

        Name = name;
        Description = description;
        Price = price;
        ImageUrl = imageUrl;
        Stock = 0;
        IsAvailable = false;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ProductDomainException("Product name is required.");

        Name = name;
    }

    public void ChangeDescription(string? description)
    {
        Description = description;
    }

    public void ChangePrice(decimal price)
    {
        if (price < 0)
            throw new ProductDomainException("Product price cannot be negative.");

        Price = price;
    }

    public void ChangeImage(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ProductDomainException("Product image is required.");

        ImageUrl = imageUrl;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ProductDomainException("The quantity to increase must be greater than zero.");

        Stock += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ProductDomainException("The quantity to decrease must be greater than zero.");

        if (quantity > Stock)
            throw new ProductDomainException("Stock cannot be reduced below zero.");

        Stock -= quantity;

        if (Stock == 0)
            IsAvailable = false;
    }

    public void Activate()
    {
        if (Stock == 0)
            throw new ProductDomainException("A product cannot be activated without stock.");

        if (Category is null)
            throw new ProductDomainException("A product cannot be activated without a category.");

        IsAvailable = true;
    }

    public void Deactivate()
    {
        IsAvailable = false;
    }

    public void AssignCategory(Category category)
    {
        Category = category;
    }

    public void RemoveCategory()
    {
        Category = null;
        IsAvailable = false;
    }
}