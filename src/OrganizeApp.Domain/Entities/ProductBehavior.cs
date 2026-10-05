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
        string? imageUrl)
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

    // Name
    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ProductDomainException("Product name is required.");

        Name = name;
    }

    // Description
    public void ChangeDescription(string? description)
    {
        Description = description;
    }

    // Price
    public void ChangePrice(decimal price)
    {
        if (price < 0)
            throw new ProductDomainException("Product price cannot be negative.");

        Price = price;
    }

    // Image
    public void ChangeImage(string? imageUrl)
    {
        ImageUrl = imageUrl;
    }

    //public void UpdateProduct(Product product)
    //{
    //}


    // Stock
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

    // Available
    public void Activate()
    {
        if (Stock <= 0)
            throw new ProductDomainException("A product cannot be activated without stock.");

        if (Categories.Count == 0)
        {
            throw new ProductDomainException("A product cannot be activated without a category.");
        }

        IsAvailable = true;
    }

    public void Deactivate()
    {
        IsAvailable = false;
    }

    // Category
    public void AssignCategory(Category category)
    {
        if (Categories.Any(c => c.Id == category.Id))
            return;

        Categories.Add(category);
    }

    public void RemoveCategory(Category category)
    {
        var existingCategory = Categories
            .FirstOrDefault(c => c.Id == category.Id);

        if (existingCategory is null)
            return;

        Categories.Remove(existingCategory);

        if (Categories.Count == 0)
            IsAvailable = false;
    }
}