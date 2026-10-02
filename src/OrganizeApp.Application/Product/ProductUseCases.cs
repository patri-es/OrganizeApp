using OrganizeApp.Application.Category.DTOs;
using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Application.Product.Interfaces;
using OrganizeApp.Application.Category.Interfaces;

namespace OrganizeApp.Application.Product;

public class ProductUseCases(IProductRepository productRepository, ICategoryRepository categoryRepository)
{

    // Constructor
    private readonly IProductRepository _productRepository = productRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    // Queries
    public async Task<List<ProductDTO>> GetProducts(FiltersProductDTO? filters)
    {
        var products = await _productRepository.GetProductsAsync(filters);

        return [.. products
            .Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Stock = product.Stock,
                IsAvailable = product.IsAvailable,
                Categories = [.. product.Categories
                    .Select(category => new CategoryDTO
                    {
                        Id = category.Id,
                        Code = category.Code,
                        Name = category.Name
                    })]
            })];
    }

    public async Task<ProductDTO?> GetProductById(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            return null; //new Exception("Product not found");

        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Stock = product.Stock,
            IsAvailable = product.IsAvailable,
            Categories = [.. product.Categories
                .Select(category => new CategoryDTO
                {
                    Id = category.Id,
                    Code = category.Code,
                    Name = category.Name
                })]
        };
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
        var product = await _productRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");
        await _productRepository.DeleteAsync(product);
        await _productRepository.SaveChangesAsync();
    }

    // Edition
    public async Task ChangeProductName(int productId, string name)
    {
        var product = await _productRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");
        product.ChangeName(name);

        await _productRepository.SaveChangesAsync();
    }

    public async Task ChangeProductDescription(int productId, string? description)
    {
        var product = await _productRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");
        product.ChangeDescription(description);

        await _productRepository.SaveChangesAsync();
    }

    public async Task ChangeProductPrice(int productId, decimal price)
    {
        var product = await _productRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");
        product.ChangePrice(price);

        await _productRepository.SaveChangesAsync();
    }

    public async Task ChangeProductImage(int productId, string imageUrl)
    {
        var product = await _productRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");
        product.ChangeImage(imageUrl);

        await _productRepository.SaveChangesAsync();
    }


    // Stock

    public async Task IncreaseProductStock(int productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");
        product.IncreaseStock(quantity);

        await _productRepository.SaveChangesAsync();
    }

    public async Task DecreaseProductStock(int productId, int quantity)
    {
        var product = await _productRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");
        product.DecreaseStock(quantity);

        await _productRepository.SaveChangesAsync();
    }


    // Availability

    public async Task ActivateProduct(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");
        product.Activate();

        await _productRepository.SaveChangesAsync();
    }

    public async Task DeactivateProduct(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");
        product.Deactivate();

        await _productRepository.SaveChangesAsync();
    }


    // Categories

    public async Task AssignProductCategory(int productId, int categoryId)
    {
        var product = await _productRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");
        var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new Exception("Category not found");
        product.AssignCategory(category);

        await _productRepository.SaveChangesAsync();
    }

    public async Task RemoveProductCategory(int productId, int categoryId)
    {
        var product = await _productRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");
        var category = await _categoryRepository.GetByIdAsync(categoryId) ?? throw new Exception("Category not found");
        product.RemoveCategory(category);

        await _productRepository.SaveChangesAsync();
    }

}