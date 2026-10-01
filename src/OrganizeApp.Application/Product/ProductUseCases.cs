using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Domain;
using System.Xml.Linq;

namespace OrganizeApp.Application.Product;

public class ProductUseCases
{

    // Constructor
    private readonly IProductRepository _productRepository;

    public ProductUseCases(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // Queries
    public async Task GetProducts()
    {
        throw new NotImplementedException();
    }

    public async Task GetProductById(int productId)
    {
        throw new NotImplementedException();
    }


    // Creation
    public async Task CreateProduct(CreateProductDTO dto)
    {
        var product = new Domain.Entities.Product(
            dto.Name,
            dto.Description,
            dto.Price,
            dto.ImageUrl);

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();
        //throw new NotImplementedException();
    }

    // Deletion
    public async Task DeleteProduct(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new Exception("Product not found");

        await _productRepository.DeleteAsync(product);
        await _productRepository.SaveChangesAsync();
    }

    // Edition
    public async Task ChangeProductName(int productId, string name)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new Exception("Product not found");

        product.ChangeName(name);

        await _productRepository.SaveChangesAsync();
    }

    public async Task ChangeProductDescription(int productId, string? description)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new Exception("Product not found");

        product.ChangeDescription(description);

        await _productRepository.SaveChangesAsync();
    }

    public async Task ChangeProductPrice(int productId, decimal price)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new Exception("Product not found");

        product.ChangePrice(price);

        await _productRepository.SaveChangesAsync();
    }

    public async Task ChangeProductImage(int productId, string imageUrl)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new Exception("Product not found");

        product.ChangeImage(imageUrl);

        await _productRepository.SaveChangesAsync();
    }


    // Stock

    public async Task IncreaseProductStock(int productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new Exception("Product not found");

        product.IncreaseStock(quantity);

        await _productRepository.SaveChangesAsync();
    }

    public async Task DecreaseProductStock(int productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new Exception("Product not found");

        product.DecreaseStock(quantity);

        await _productRepository.SaveChangesAsync();
    }


    // Availability

    public async Task ActivateProduct(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new Exception("Product not found");

        product.Activate();

        await _productRepository.SaveChangesAsync();
    }

    public async Task DeactivateProduct(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new Exception("Product not found");

        product.Deactivate();

        await _productRepository.SaveChangesAsync();
    }


    // Categories

    public async Task AssignProductCategory(int productId, int categoryId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new Exception("Product not found");

        // TODO get Category by Id.
    }

    public async Task RemoveProductCategory(int productId, int categoryId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            throw new Exception("Product not found");

        // TODO get Category by Id.
        // TODO remove relation between Product and Category
    }

}