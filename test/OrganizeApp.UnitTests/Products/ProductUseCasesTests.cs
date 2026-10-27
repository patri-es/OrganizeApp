using OrganizeApp.Application.Category.Interfaces;
using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Application.Product.Interfaces;
using OrganizeApp.Domain.Entities;
using ProductEntity = OrganizeApp.Domain.Entities.Product;
using ProductUseCases = OrganizeApp.Application.Product.ProductUseCases;

namespace OrganizeApp.UnitTests.Products;

public class ProductUseCasesTests
{
    #region Queries of products

    [Fact]
    public async Task GetProducts_WithProducts_ReturnsMappedProductDTOs()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");

        var productRepository = new FakeProductRepository
        {
            Products = [product]
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProducts(null);

        // Assert
        var productDTO = Assert.Single(result);
        Assert.Equal(1, productDTO.Id);
        Assert.Equal("Keyboard", productDTO.Name);
        Assert.Equal("Mechanical keyboard", productDTO.Description);
        Assert.Equal(59.99m, productDTO.Price);
        Assert.Equal("keyboard.jpg", productDTO.ImageUrl);
        Assert.Equal(product.Stock, productDTO.Stock);
        Assert.Equal(product.IsAvailable, productDTO.IsAvailable);
    }

    [Fact]
    public async Task GetProducts_WithAssociatedCategories_ReturnsMappedCategories()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");
        var category = CreateCategory(10, "Peripherals", "PERIPHERALS");

        product.AssignCategory(category);

        var productRepository = new FakeProductRepository
        {
            Products = [product]
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProducts(null);

        // Assert
        var productDTO = Assert.Single(result);
        var categoryDTO = Assert.Single(productDTO.Categories);

        Assert.Equal(10, categoryDTO.Id);
        Assert.Equal("PERIPHERALS", categoryDTO.Code);
        Assert.Equal("Peripherals", categoryDTO.Name);
    }

    [Fact]
    public async Task GetProducts_WhenNoProductsExist_ReturnsEmptyList()
    {
        // Arrange
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProducts(null);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetProducts_WithFilters_PassesFiltersToRepository()
    {
        // Arrange
        var filters = new FiltersProductDTO();
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        await useCases.GetProducts(filters);

        // Assert
        Assert.Same(filters, productRepository.ReceivedFilters);
    }

    [Fact]
    public async Task GetProductById_WhenProductExists_ReturnsMappedProductDTO()
    {
        // Arrange
        var product = CreateProduct(5, "Mouse", "Wireless mouse", 29.99m, "mouse.jpg");

        var productRepository = new FakeProductRepository
        {
            ProductsById = { [5] = product }
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProductById(5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Id);
        Assert.Equal("Mouse", result.Name);
        Assert.Equal("Wireless mouse", result.Description);
        Assert.Equal(29.99m, result.Price);
        Assert.Equal("mouse.jpg", result.ImageUrl);
        Assert.Equal(product.Stock, result.Stock);
        Assert.Equal(product.IsAvailable, result.IsAvailable);
    }

    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        var productRepository = new FakeProductRepository();
        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProductById(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProducts_WithProductWithoutCategories_ReturnsEmptyCategoryList()
    {
        // Arrange
        var product = CreateProduct(1, "Keyboard", "Mechanical keyboard", 59.99m, "keyboard.jpg");

        var productRepository = new FakeProductRepository
        {
            Products = [product]
        };

        var useCases = CreateUseCases(productRepository);

        // Act
        var result = await useCases.GetProducts(null);

        // Assert
        var productDTO = Assert.Single(result);
        Assert.Empty(productDTO.Categories);
    }

    #endregion

    #region Test helpers

    private static ProductUseCases CreateUseCases(
        FakeProductRepository productRepository)
    {
        return new ProductUseCases(
            productRepository,
            new FakeCategoryRepository());
    }

    private static ProductEntity CreateProduct(
        int id,
        string name,
        string description,
        decimal price,
        string imageUrl)
    {
        var product = new ProductEntity(name, description, price, imageUrl);
        SetEntityId(product, id);
        return product;
    }

    private static Category CreateCategory(
        int id,
        string name,
        string code)
    {
        var category = new Category(name, code, "Test description", "category.jpg");
        SetEntityId(category, id);
        return category;
    }

    private static void SetEntityId(object entity, int id)
    {
        entity.GetType()
            .GetProperty("Id")!
            .SetValue(entity, id);
    }

    #endregion

    #region Fake repositories

    private sealed class FakeProductRepository : IProductRepository
    {
        public List<ProductEntity> Products { get; set; } = [];

        public Dictionary<int, ProductEntity> ProductsById { get; } = [];

        public FiltersProductDTO? ReceivedFilters { get; private set; }

        public Task<List<ProductEntity>> GetProductsAsync(FiltersProductDTO? filters)
        {
            ReceivedFilters = filters;
            return Task.FromResult(Products);
        }

        public Task<ProductEntity?> GetByIdAsync(int productId)
        {
            ProductsById.TryGetValue(productId, out var product);
            return Task.FromResult(product);
        }

        public Task<ProductEntity?> GetByIdWithCategoriesAsync(
            int id,
            CancellationToken cancellationToken)
        {
            return GetByIdAsync(id);
        }

        public Task AddAsync(ProductEntity product) => Task.CompletedTask;

        public Task DeleteAsync(ProductEntity product) => Task.CompletedTask;

        public Task SaveChangesAsync() => Task.CompletedTask;
    }

    private sealed class FakeCategoryRepository : ICategoryRepository
    {
        public Task<List<Category>> GetCategoriesAsync()
        {
            return Task.FromResult(new List<Category>());
        }

        public Task<Category?> GetByIdAsync(int categoryId)
        {
            return Task.FromResult<Category?>(null);
        }

        public Task AddAsync(Category category) => Task.CompletedTask;

        public Task DeleteAsync(Category category) => Task.CompletedTask;

        public Task SaveChangesAsync() => Task.CompletedTask;
    }

    #endregion
}