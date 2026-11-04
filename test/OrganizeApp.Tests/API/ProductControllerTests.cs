
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Domain.Entities;
using OrganizeApp.Infrastructure.Persistence;
using OrganizeApp.Tests.Infrastructure;

namespace OrganizeApp.Tests.API;

public class ProductControllerTests(DatabaseFixture databaseFixture) : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _databaseFixture = databaseFixture;

    #region Product Queries

    [Fact]
    public async Task GetProducts_WithProducts_ReturnsOkWithProductDTOs()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        var category = CreateCategory("Electronics", "ELECTRONICS");
        product.AssignCategory(category);

        await SaveProductsAsync(product, CreateProduct("Mouse"));

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.GetAsync(
            "/api/products",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<ProductDTO>>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(products);
        Assert.Equal(2, products.Count);

        var keyboard = Assert.Single(
            products,
            p => p.Name == "Keyboard");

        Assert.Equal(25m, keyboard.Price);
        Assert.Equal("Keyboard description", keyboard.Description);

        var assignedCategory = Assert.Single(keyboard.Categories);

        Assert.Equal("ELECTRONICS", assignedCategory.Code);
        Assert.Equal("Electronics", assignedCategory.Name);
    }

    [Fact]
    public async Task GetProducts_WhenNoProductsExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.GetAsync(
            "/api/products",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<ProductDTO>>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(products);
        Assert.Empty(products);
    }

    [Fact]
    public async Task GetProducts_WithFilters_ReturnsMatchingProducts()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var electronics = CreateCategory("Electronics", "ELECTRONICS");
        var home = CreateCategory("Home", "HOME");

        var matchingProduct = CreateProduct("Matching Keyboard", 7);
        matchingProduct.AssignCategory(electronics);
        matchingProduct.Activate();

        var excessiveStockProduct = CreateProduct("Excessive Stock", 15);
        excessiveStockProduct.AssignCategory(electronics);
        excessiveStockProduct.Activate();

        var unavailableProduct = CreateProduct("Unavailable Keyboard", 7);
        unavailableProduct.AssignCategory(electronics);

        var differentCategoryProduct = CreateProduct("Home Product", 7);
        differentCategoryProduct.AssignCategory(home);
        differentCategoryProduct.Activate();

        await SaveProductsAsync(
            matchingProduct,
            excessiveStockProduct,
            unavailableProduct,
            differentCategoryProduct);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        var requestUri =
            $"/api/products?CategoryIds={electronics.Id}" +
            "&IsAvailable=true&MinStock=5&MaxStock=10";

        // Act
        using var response = await client.GetAsync(
            requestUri,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<ProductDTO>>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(products);

        var product = Assert.Single(products);

        Assert.Equal("Matching Keyboard", product.Name);
        Assert.True(product.IsAvailable);
        Assert.InRange(product.Stock, 5, 10);
    }

    [Fact]
    public async Task GetProduct_WhenProductExists_ReturnsOkWithProductDTO()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        var category = CreateCategory("Electronics", "ELECTRONICS");
        product.AssignCategory(category);

        await SaveProductsAsync(product);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.GetAsync(
            $"/api/products/{product.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<ProductDTO>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal("Keyboard", result.Name);
        Assert.Equal(25m, result.Price);

        var assignedCategory = Assert.Single(result.Categories);

        Assert.Equal("ELECTRONICS", assignedCategory.Code);
    }

    [Fact]
    public async Task GetProduct_WhenProductDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.GetAsync(
            "/api/products/2147483647",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Product Creation

    [Fact]
    public async Task CreateProduct_WithValidData_ReturnsOkWithProductDTO()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var dto = new CreateProductDTO
        {
            Name = "Keyboard",
            Description = "Mechanical keyboard",
            Price = 59.99m,
            ImageUrl = "keyboard.jpg"
        };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PostAsync(
            "/api/products",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var returnedProduct = await response.Content.ReadFromJsonAsync<CreateProductDTO>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(returnedProduct);
        Assert.Equal(dto.Name, returnedProduct.Name);
        Assert.Equal(dto.Description, returnedProduct.Description);
        Assert.Equal(dto.Price, returnedProduct.Price);
        Assert.Equal(dto.ImageUrl, returnedProduct.ImageUrl);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var savedProduct = await verificationContext.Products
            .AsNoTracking()
            .SingleAsync(
                p => p.Name == dto.Name,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(dto.Price, savedProduct.Price);
        Assert.Equal(dto.ImageUrl, savedProduct.ImageUrl);
    }

    [Fact]
    public async Task CreateProduct_WithInvalidData_ReturnsBadRequestProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var dto = new CreateProductDTO
        {
            Name = "Keyboard",
            Description = "Mechanical keyboard",
            Price = -1m,
            ImageUrl = "keyboard.jpg"
        };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PostAsync(
            "/api/products",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.BadRequest,
            "Business rule violation");
    }

    #endregion

    #region Product Updates

    [Fact]
    public async Task UpdateProduct_WithValidData_ReturnsOkWithUpdatedProductDTO()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        await SaveProductsAsync(product);

        var dto = new UpdateProductDTO
        {
            Name = "Gaming Keyboard",
            Description = "Updated description",
            Price = 79.99m,
            ImageUrl = "gaming-keyboard.jpg"
        };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(
            HttpMethod.Patch,
            $"/api/products/{product.Id}")
        {
            Content = JsonContent.Create(dto)
        };

        // Act
        using var response = await client.SendAsync(
            request,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var returnedProduct = await response.Content.ReadFromJsonAsync<UpdateProductDTO>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(returnedProduct);
        Assert.Equal(dto.Name, returnedProduct.Name);
        Assert.Equal(dto.Price, returnedProduct.Price);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var savedProduct = await verificationContext.Products
            .AsNoTracking()
            .SingleAsync(
                p => p.Id == product.Id,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(dto.Name, savedProduct.Name);
        Assert.Equal(dto.Description, savedProduct.Description);
        Assert.Equal(dto.Price, savedProduct.Price);
        Assert.Equal(dto.ImageUrl, savedProduct.ImageUrl);
    }

    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var dto = new UpdateProductDTO
        {
            Name = "Gaming Keyboard",
            Description = "Updated description",
            Price = 79.99m,
            ImageUrl = "gaming-keyboard.jpg"
        };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(
            HttpMethod.Patch,
            "/api/products/2147483647")
        {
            Content = JsonContent.Create(dto)
        };

        // Act
        using var response = await client.SendAsync(
            request,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    [Fact]
    public async Task UpdateProduct_WithInvalidData_ReturnsBadRequestProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        await SaveProductsAsync(product);

        var dto = new UpdateProductDTO
        {
            Name = "Gaming Keyboard",
            Description = "Updated description",
            Price = -1m,
            ImageUrl = "gaming-keyboard.jpg"
        };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(
            HttpMethod.Patch,
            $"/api/products/{product.Id}")
        {
            Content = JsonContent.Create(dto)
        };

        // Act
        using var response = await client.SendAsync(
            request,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.BadRequest,
            "Business rule violation");
    }

    #endregion

    #region Product Deletion

    [Fact]
    public async Task DeleteProduct_WhenProductExists_ReturnsOk()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        await SaveProductsAsync(product);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.DeleteAsync(
            $"/api/products/{product.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var deletedProduct = await verificationContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.Id == product.Id,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.DeleteAsync(
            "/api/products/2147483647",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    #endregion

    #region Product Stock Management

    [Fact]
    public async Task IncreaseStock_WithValidQuantity_ReturnsOk()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard", 5);
        await SaveProductsAsync(product);

        var dto = new ChangeStockDTO { Quantity = 3 };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PostAsync(
            $"/api/products/{product.Id}/stock/increase",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var savedProduct = await verificationContext.Products
            .AsNoTracking()
            .SingleAsync(
                p => p.Id == product.Id,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(8, savedProduct.Stock);
    }

    [Fact]
    public async Task IncreaseStock_WhenProductDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var dto = new ChangeStockDTO { Quantity = 3 };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PostAsync(
            "/api/products/2147483647/stock/increase",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    [Fact]
    public async Task IncreaseStock_WithInvalidQuantity_ReturnsBadRequestProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        await SaveProductsAsync(product);

        var dto = new ChangeStockDTO { Quantity = -1 };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PostAsync(
            $"/api/products/{product.Id}/stock/increase",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.BadRequest,
            "Business rule violation");
    }

    [Fact]
    public async Task DecreaseStock_WithValidQuantity_ReturnsOk()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard", 10);
        await SaveProductsAsync(product);

        var dto = new ChangeStockDTO { Quantity = 3 };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PostAsync(
            $"/api/products/{product.Id}/stock/decrease",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var savedProduct = await verificationContext.Products
            .AsNoTracking()
            .SingleAsync(
                p => p.Id == product.Id,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(7, savedProduct.Stock);
    }

    [Fact]
    public async Task DecreaseStock_WhenProductDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var dto = new ChangeStockDTO { Quantity = 3 };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PostAsync(
            "/api/products/2147483647/stock/decrease",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    [Fact]
    public async Task DecreaseStock_WithInvalidQuantity_ReturnsBadRequestProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard", 5);
        await SaveProductsAsync(product);

        var dto = new ChangeStockDTO { Quantity = 6 };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PostAsync(
            $"/api/products/{product.Id}/stock/decrease",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.BadRequest,
            "Business rule violation");
    }

    #endregion

    #region Product Availability

    [Fact]
    public async Task ActivateProduct_WithStockAndCategories_ReturnsOk()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var category = CreateCategory("Electronics", "ELECTRONICS");
        var product = CreateProduct("Keyboard", 5);

        product.AssignCategory(category);

        await SaveProductsAsync(product);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(
            $"/api/products/{product.Id}/activate",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var savedProduct = await verificationContext.Products
            .AsNoTracking()
            .SingleAsync(
                p => p.Id == product.Id,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.True(savedProduct.IsAvailable);
    }

    [Fact]
    public async Task ActivateProduct_WhenProductDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(
            "/api/products/2147483647/activate",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public async Task ActivateProduct_WithoutStockOrCategories_ReturnsBadRequestProblemDetails(
        bool hasStock,
        bool hasCategory)
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct(
            "Keyboard",
            hasStock ? 5 : 0);

        if (hasCategory)
        {
            product.AssignCategory(
                CreateCategory("Electronics", "ELECTRONICS"));
        }

        await SaveProductsAsync(product);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(
            $"/api/products/{product.Id}/activate",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.BadRequest,
            "Business rule violation");
    }

    [Fact]
    public async Task DeactivateProduct_WithExistingProduct_ReturnsOk()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var category = CreateCategory("Electronics", "ELECTRONICS");
        var product = CreateProduct("Keyboard", 5);

        product.AssignCategory(category);
        product.Activate();

        await SaveProductsAsync(product);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(
            $"/api/products/{product.Id}/deactivate",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var savedProduct = await verificationContext.Products
            .AsNoTracking()
            .SingleAsync(
                p => p.Id == product.Id,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.False(savedProduct.IsAvailable);
    }

    [Fact]
    public async Task DeactivateProduct_WhenProductDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(
            "/api/products/2147483647/deactivate",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    #endregion

    #region Product Category Management

    [Fact]
    public async Task AssignProductCategory_WithExistingProductAndCategory_ReturnsOk()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        var category = CreateCategory("Electronics", "ELECTRONICS");

        await SaveProductsAsync(product);
        await SaveCategoriesAsync(category);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(
            $"/api/products/{product.Id}/categories/{category.Id}",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var savedProduct = await verificationContext.Products
            .Include(p => p.Categories)
            .AsNoTracking()
            .SingleAsync(
                p => p.Id == product.Id,
                cancellationToken: TestContext.Current.CancellationToken);

        var assignedCategory = Assert.Single(savedProduct.Categories);

        Assert.Equal(category.Id, assignedCategory.Id);
    }

    [Fact]
    public async Task AssignProductCategory_WhenProductDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var category = CreateCategory("Electronics", "ELECTRONICS");
        await SaveCategoriesAsync(category);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(
            $"/api/products/2147483647/categories/{category.Id}",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    [Fact]
    public async Task AssignProductCategory_WhenCategoryDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        await SaveProductsAsync(product);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(
            $"/api/products/{product.Id}/categories/2147483647",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    [Fact]
    public async Task AssignProductCategory_WhenCategoryIsAlreadyAssigned_ReturnsBadRequestProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        var category = CreateCategory("Electronics", "ELECTRONICS");

        product.AssignCategory(category);

        await SaveProductsAsync(product);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.PostAsync(
            $"/api/products/{product.Id}/categories/{category.Id}",
            null,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.BadRequest,
            "Business rule violation");
    }

    [Fact]
    public async Task RemoveProductCategory_WithExistingAssociation_ReturnsOk()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        var category = CreateCategory("Electronics", "ELECTRONICS");

        product.AssignCategory(category);

        await SaveProductsAsync(product);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.DeleteAsync(
            $"/api/products/{product.Id}/categories/{category.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var savedProduct = await verificationContext.Products
            .Include(p => p.Categories)
            .AsNoTracking()
            .SingleAsync(
                p => p.Id == product.Id,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Empty(savedProduct.Categories);
    }

    [Fact]
    public async Task RemoveProductCategory_WhenProductDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var category = CreateCategory("Electronics", "ELECTRONICS");
        await SaveCategoriesAsync(category);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.DeleteAsync(
            $"/api/products/2147483647/categories/{category.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    [Fact]
    public async Task RemoveProductCategory_WhenCategoryDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        await SaveProductsAsync(product);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.DeleteAsync(
            $"/api/products/{product.Id}/categories/2147483647",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    [Fact]
    public async Task RemoveProductCategory_WhenCategoryIsNotAssigned_ReturnsBadRequestProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var product = CreateProduct("Keyboard");
        var category = CreateCategory("Electronics", "ELECTRONICS");

        await SaveProductsAsync(product);
        await SaveCategoriesAsync(category);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.DeleteAsync(
            $"/api/products/{product.Id}/categories/{category.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.BadRequest,
            "Business rule violation");
    }

    #endregion

    #region Test Helpers

    // Creates a product with default test values and an optional initial stock.
    // Uses the domain method to initialize stock when a positive quantity is provided.
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

    // Creates a category with default description and image values for test scenarios.
    private static Category CreateCategory(string name, string code)
    {
        return new Category(
            name,
            code,
            "Test description",
            "category.jpg");
    }

    // Saves products using a separate DbContext to persist test data.
    // This allows the test to verify the results through another context.
    private async Task SaveProductsAsync(params Product[] products)
    {
        await using AppDbContext context = _databaseFixture.CreateContext();

        await context.Products.AddRangeAsync(products,TestContext.Current.CancellationToken);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
    }

    // Saves categories using a separate DbContext to persist test data.
    // This is useful when categories must exist before sending an API request.
    private async Task SaveCategoriesAsync(params Category[] categories)
    {
        await using AppDbContext context = _databaseFixture.CreateContext();

        await context.Categories.AddRangeAsync( categories, TestContext.Current.CancellationToken);

        await context.SaveChangesAsync( TestContext.Current.CancellationToken);
    }

    // Verifies the HTTP status code and the main properties of a ProblemDetails response.
    // Centralizes error response assertions to avoid repeating the same checks in multiple tests.
    private static async Task AssertProblemDetailsAsync( HttpResponseMessage response, HttpStatusCode expectedStatus, string expectedTitle)
    {
        Assert.Equal(expectedStatus, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(problemDetails);
        Assert.Equal((int?)expectedStatus, problemDetails.Status);
        Assert.Equal(expectedTitle, problemDetails.Title);
    }

    #endregion
}
