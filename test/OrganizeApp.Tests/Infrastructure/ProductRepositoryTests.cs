
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Domain.Entities;
using OrganizeApp.Infrastructure.Persistence;
using OrganizeApp.Infrastructure.Persistence.Repositories;

namespace OrganizeApp.Tests.Infrastructure;

public class ProductRepositoryTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture = fixture;

    #region Product Queries

    [Fact]
    public async Task GetProducts_WithProducts_ReturnsAllProducts()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var products = new[]
            {
                CreateProduct("Keyboard"),
                CreateProduct("Mouse")
            };

            await AddProductsAsync(arrangeContext, products);
        }

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductsAsync(null);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, product => product.Name == "Keyboard");
        Assert.Contains(result, product => product.Name == "Mouse");
    }

    [Fact]
    public async Task GetProducts_WhenNoProductsExist_ReturnsEmptyList()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        // Act
        var result = await repository.GetProductsAsync(null);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetProducts_WithAssociatedCategories_IncludesCategories()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        int productId;

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var category = CreateCategory("Electronics", "ELECTRONICS");
            await AddCategoriesAsync(arrangeContext, category);

            var product = CreateProduct("Keyboard");
            product.AssignCategory(category);

            await AddProductsAsync(arrangeContext, product);
            productId = product.Id;
        }

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductsAsync(null);

        // Assert
        var savedProduct = Assert.Single(
            result,
            product => product.Id == productId);

        var assignedCategory = Assert.Single(savedProduct.Categories);

        Assert.Equal("ELECTRONICS", assignedCategory.Code);
        Assert.Equal("Electronics", assignedCategory.Name);
    }

    [Fact]
    public async Task GetById_WhenProductExists_ReturnsProductWithCategories()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        int productId;

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var category = CreateCategory("Electronics", "ELECTRONICS");
            await AddCategoriesAsync(arrangeContext, category);

            var product = CreateProduct("Keyboard");
            product.AssignCategory(category);

            await AddProductsAsync(arrangeContext, product);
            productId = product.Id;
        }

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal("Keyboard", result.Name);

        var assignedCategory = Assert.Single(result.Categories);

        Assert.Equal("ELECTRONICS", assignedCategory.Code);
        Assert.Equal("Electronics", assignedCategory.Name);
    }

    [Fact]
    public async Task GetById_WhenProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        // Act
        var result = await repository.GetByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region Product Filters

    [Fact]
    public async Task GetProducts_WithCategoryIdsFilter_ReturnsProductsMatchingAnyCategoryId()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        int firstCategoryId;
        int secondCategoryId;

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var firstCategory = CreateCategory("Electronics", "ELECTRONICS");
            var secondCategory = CreateCategory("Home", "HOME");
            var thirdCategory = CreateCategory("Garden", "GARDEN");

            await AddCategoriesAsync(
                arrangeContext,
                firstCategory,
                secondCategory,
                thirdCategory);

            firstCategoryId = firstCategory.Id;
            secondCategoryId = secondCategory.Id;

            var keyboard = CreateProduct("Keyboard");
            keyboard.AssignCategory(firstCategory);

            var chair = CreateProduct("Chair");
            chair.AssignCategory(secondCategory);

            var lawnMower = CreateProduct("Lawn Mower");
            lawnMower.AssignCategory(thirdCategory);

            var book = CreateProduct("Book");

            await AddProductsAsync(
                arrangeContext,
                keyboard,
                chair,
                lawnMower,
                book);
        }

        var filters = new FiltersProductDTO
        {
            CategoryIds = [firstCategoryId, secondCategoryId]
        };

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductsAsync(filters);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, product => product.Name == "Keyboard");
        Assert.Contains(result, product => product.Name == "Chair");
        Assert.DoesNotContain(result, product => product.Name == "Lawn Mower");
        Assert.DoesNotContain(result, product => product.Name == "Book");
    }

    [Fact]
    public async Task GetProducts_WithHasCategoryTrueFilter_ReturnsOnlyProductsWithCategories()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var category = CreateCategory("Electronics", "ELECTRONICS");
            await AddCategoriesAsync(arrangeContext, category);

            var keyboard = CreateProduct("Keyboard");
            keyboard.AssignCategory(category);

            var book = CreateProduct("Book");

            await AddProductsAsync(arrangeContext, keyboard, book);
        }

        var filters = new FiltersProductDTO
        {
            HasCategory = true
        };

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductsAsync(filters);

        // Assert
        var product = Assert.Single(result);
        Assert.Equal("Keyboard", product.Name);
        Assert.NotEmpty(product.Categories);
    }

    [Fact]
    public async Task GetProducts_WithHasCategoryFalseFilter_ReturnsOnlyProductsWithoutCategories()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var category = CreateCategory("Electronics", "ELECTRONICS");
            await AddCategoriesAsync(arrangeContext, category);

            var keyboard = CreateProduct("Keyboard");
            keyboard.AssignCategory(category);

            var book = CreateProduct("Book");

            await AddProductsAsync(arrangeContext, keyboard, book);
        }

        var filters = new FiltersProductDTO
        {
            HasCategory = false
        };

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductsAsync(filters);

        // Assert
        var product = Assert.Single(result);
        Assert.Equal("Book", product.Name);
        Assert.Empty(product.Categories);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetProducts_WithIsAvailableFilter_ReturnsProductsMatchingAvailability(
        bool isAvailable)
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var category = CreateCategory("Electronics", "ELECTRONICS");
            await AddCategoriesAsync(arrangeContext, category);

            var availableProduct = CreateProduct("Available Keyboard", 5);
            availableProduct.AssignCategory(category);
            availableProduct.Activate();

            var unavailableProduct = CreateProduct("Unavailable Keyboard", 5);
            unavailableProduct.AssignCategory(category);

            await AddProductsAsync(
                arrangeContext,
                availableProduct,
                unavailableProduct);
        }

        var filters = new FiltersProductDTO
        {
            IsAvailable = isAvailable
        };

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductsAsync(filters);

        // Assert
        var product = Assert.Single(result);

        Assert.Equal(
            isAvailable ? "Available Keyboard" : "Unavailable Keyboard",
            product.Name);

        Assert.Equal(isAvailable, product.IsAvailable);
    }

    [Fact]
    public async Task GetProducts_WithMinStockFilter_ReturnsProductsWithStockAtOrAboveMinimum()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var products = new[]
            {
                CreateProduct("Keyboard", 0),
                CreateProduct("Mouse", 5),
                CreateProduct("Monitor", 10)
            };

            await AddProductsAsync(arrangeContext, products);
        }

        var filters = new FiltersProductDTO
        {
            MinStock = 5
        };

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductsAsync(filters);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, product => product.Name == "Mouse");
        Assert.Contains(result, product => product.Name == "Monitor");
        Assert.DoesNotContain(result, product => product.Name == "Keyboard");
    }

    [Fact]
    public async Task GetProducts_WithMaxStockFilter_ReturnsProductsWithStockAtOrBelowMaximum()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var products = new[]
            {
                CreateProduct("Keyboard", 0),
                CreateProduct("Mouse", 5),
                CreateProduct("Monitor", 10)
            };

            await AddProductsAsync(arrangeContext, products);
        }

        var filters = new FiltersProductDTO
        {
            MaxStock = 5
        };

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductsAsync(filters);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, product => product.Name == "Keyboard");
        Assert.Contains(result, product => product.Name == "Mouse");
        Assert.DoesNotContain(result, product => product.Name == "Monitor");
    }

    [Fact]
    public async Task GetProducts_WithCombinedFilters_ReturnsMatchingProducts()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        int targetCategoryId;

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var targetCategory = CreateCategory("Electronics", "ELECTRONICS");
            var otherCategory = CreateCategory("Home", "HOME");

            await AddCategoriesAsync(
                arrangeContext,
                targetCategory,
                otherCategory);

            targetCategoryId = targetCategory.Id;

            var matchingProduct = CreateProduct("Matching Keyboard", 7);
            matchingProduct.AssignCategory(targetCategory);
            matchingProduct.Activate();

            var excessiveStockProduct = CreateProduct("Excessive Stock", 15);
            excessiveStockProduct.AssignCategory(targetCategory);
            excessiveStockProduct.Activate();

            var unavailableProduct = CreateProduct("Unavailable Keyboard", 7);
            unavailableProduct.AssignCategory(targetCategory);

            var differentCategoryProduct = CreateProduct("Home Product", 7);
            differentCategoryProduct.AssignCategory(otherCategory);
            differentCategoryProduct.Activate();

            await AddProductsAsync(
                arrangeContext,
                matchingProduct,
                excessiveStockProduct,
                unavailableProduct,
                differentCategoryProduct);
        }

        var filters = new FiltersProductDTO
        {
            CategoryIds = [targetCategoryId],
            HasCategory = true,
            IsAvailable = true,
            MinStock = 5,
            MaxStock = 10
        };

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductsAsync(filters);

        // Assert
        var product = Assert.Single(result);

        Assert.Equal("Matching Keyboard", product.Name);
        Assert.True(product.IsAvailable);
        Assert.InRange(product.Stock, 5, 10);
    }

    [Fact]
    public async Task GetProducts_WithEmptyCategoryIdsFilter_ReturnsAllProducts()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var products = new[]
            {
                CreateProduct("Keyboard"),
                CreateProduct("Mouse")
            };

            await AddProductsAsync(arrangeContext, products);
        }

        var filters = new FiltersProductDTO
        {
            CategoryIds = []
        };

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetProductsAsync(filters);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, product => product.Name == "Keyboard");
        Assert.Contains(result, product => product.Name == "Mouse");
    }

    #endregion

    #region Product Persistence

    [Fact]
    public async Task AddAsync_AndSaveChangesAsync_WithValidProduct_PersistsProduct()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard", 10);

        await using (var context = _fixture.CreateContext())
        {
            var repository = new ProductRepository(context);

            // Act
            await repository.AddAsync(product);
            await repository.SaveChangesAsync();
        }

        // Assert
        await using var verificationContext = _fixture.CreateContext();

        var savedProduct = await verificationContext.Products
            .AsNoTracking()
            .SingleAsync(p => p.Id == product.Id, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("Keyboard", savedProduct.Name);
        Assert.Equal(10, savedProduct.Stock);
        Assert.Equal(25m, savedProduct.Price);
    }

    [Fact]
    public async Task SaveChangesAsync_WithModifiedProduct_PersistsChanges()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");

        await using (var context = _fixture.CreateContext())
        {
            var repository = new ProductRepository(context);

            await repository.AddAsync(product);
            await repository.SaveChangesAsync();

            // Act
            product.ChangeName("Gaming Keyboard");
            product.ChangePrice(79.99m);

            await repository.SaveChangesAsync();
        }

        // Assert
        await using var verificationContext = _fixture.CreateContext();

        var savedProduct = await verificationContext.Products
            .AsNoTracking()
            .SingleAsync(p => p.Id == product.Id, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("Gaming Keyboard", savedProduct.Name);
        Assert.Equal(79.99m, savedProduct.Price);
    }

    [Fact]
    public async Task DeleteAsync_AndSaveChangesAsync_WithExistingProduct_DeletesProduct()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        int productId;

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var product = CreateProduct("Keyboard");

            arrangeContext.Products.Add(product);
            await arrangeContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            productId = product.Id;
        }

        // Act
        await using (var context = _fixture.CreateContext())
        {
            var repository = new ProductRepository(context);

            var product = await repository.GetByIdAsync(productId);

            Assert.NotNull(product);

            await repository.DeleteAsync(product);
            await repository.SaveChangesAsync();
        }

        // Assert
        await using AppDbContext verificationContext = _fixture.CreateContext();

        var deletedProduct = await verificationContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken: TestContext.Current.CancellationToken);

        Assert.Null(deletedProduct);
    }

    #endregion

    #region Product-Category Relationships

    [Fact]
    public async Task AddAsync_WithAssociatedCategory_PersistsProductCategoryRelationship()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");

        await using (var context = _fixture.CreateContext())
        {
            var category = CreateCategory("Electronics", "ELECTRONICS");

            await AddCategoriesAsync(context, category);

            product.AssignCategory(category);

            var repository = new ProductRepository(context);

            // Act
            await repository.AddAsync(product);
            await repository.SaveChangesAsync();
        }

        // Assert
        await using var verificationContext = _fixture.CreateContext();

        var savedProduct = await verificationContext.Products
            .Include(p => p.Categories)
            .SingleAsync(p => p.Id == product.Id, cancellationToken: TestContext.Current.CancellationToken);

        var assignedCategory = Assert.Single(savedProduct.Categories);

        Assert.Equal("ELECTRONICS", assignedCategory.Code);
        Assert.Equal("Electronics", assignedCategory.Name);
    }

    [Fact]
    public async Task GetById_WhenProductHasMultipleCategories_LoadsAllCategories()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        int productId;

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var firstCategory = CreateCategory("Electronics", "ELECTRONICS");
            var secondCategory = CreateCategory("Home", "HOME");

            await AddCategoriesAsync(
                arrangeContext,
                firstCategory,
                secondCategory);

            var product = CreateProduct("Keyboard");
            product.AssignCategory(firstCategory);
            product.AssignCategory(secondCategory);

            await AddProductsAsync(arrangeContext, product);

            productId = product.Id;
        }

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new ProductRepository(context);

        var result = await repository.GetByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Categories.Count);

        Assert.Contains(
            result.Categories,
            category => category.Code == "ELECTRONICS");

        Assert.Contains(
            result.Categories,
            category => category.Code == "HOME");
    }

    #endregion

    #region Test Helpers

    private static Product CreateProduct(string name, int stock = 0)
    {
        var product = new Product(
            name,
            $"{name} description",
            25m,
            "product.jpg");

        if (stock > 0)
            product.IncreaseStock(stock);

        return product;
    }

    private static Category CreateCategory(string name, string code)
    {
        return new Category(
            name,
            code,
            "Test description",
            "category.jpg");
    }

    private static async Task AddCategoriesAsync(AppDbContext context, params Category[] categories)
    {
        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static async Task AddProductsAsync(AppDbContext context, params Product[] products)
    {
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }

    #endregion
}
