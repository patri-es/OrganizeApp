using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Domain.Entities;

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
        var product = new CreateProductDTO(
            dto.Name,
            dto.Description,
            dto.Price,
            dto.ImageUrl);

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public async Task ChangeProductPrice(int productId, decimal price)
    {
        throw new NotImplementedException();
    }

    public async Task ChangeProductImage(int productId, string imageUrl)
    {
        throw new NotImplementedException();
    }


    // Stock

    public async Task IncreaseProductStock(int productId, int quantity)
    {
        throw new NotImplementedException();
    }

    public async Task DecreaseProductStock(int productId, int quantity)
    {
        throw new NotImplementedException();
    }


    // Availability

    public async Task ActivateProduct(int productId)
    {
        throw new NotImplementedException();
    }

    public async Task DeactivateProduct(int productId)
    {
        throw new NotImplementedException();
    }


    // Categories

    public async Task AssignProductCategory(int productId, int categoryId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveProductCategory(int productId, int categoryId)
    {
        throw new NotImplementedException();
    }


    // Deletion

    public async Task DeleteProduct(int productId)
    {
        throw new NotImplementedException();
    }
}