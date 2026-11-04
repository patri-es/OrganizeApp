
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Category.DTOs;
using OrganizeApp.Domain.Entities;
using OrganizeApp.Infrastructure.Persistence;
using OrganizeApp.Tests.Infrastructure;

namespace OrganizeApp.Tests.API;

public class CategoryControllerTests(DatabaseFixture databaseFixture) : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _databaseFixture = databaseFixture;

    #region Category Queries

    [Fact]
    public async Task GetCategories_WithCategories_ReturnsOkWithCategorySummaryDTOs()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var categories = new[]
        {
            CreateCategory("Electronics", "ELECTRONICS"),
            CreateCategory("Home", "HOME")
        };

        await SaveCategoriesAsync(categories);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.GetAsync(
            "/api/categories",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<List<CategorySummaryDTO>>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Contains(result, category =>
            category.Name == "Electronics" &&
            category.Code == "ELECTRONICS");

        Assert.Contains(result, category =>
            category.Name == "Home" &&
            category.Code == "HOME");
    }

    [Fact]
    public async Task GetCategories_WhenNoCategoriesExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.GetAsync(
            "/api/categories",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<List<CategorySummaryDTO>>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCategory_WhenCategoryExists_ReturnsOkWithCategoryDTO()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var category = CreateCategory(
            "Electronics",
            "ELECTRONICS",
            "Electronic products",
            "electronics.jpg");

        await SaveCategoriesAsync(category);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.GetAsync(
            $"/api/categories/{category.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CategoryDTO>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
        Assert.Equal("Electronics", result.Name);
        Assert.Equal("ELECTRONICS", result.Code);
        Assert.Equal("Electronic products", result.Description);
        Assert.Equal("electronics.jpg", result.ImageUrl);
    }

    [Fact]
    public async Task GetCategory_WhenCategoryDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.GetAsync(
            "/api/categories/2147483647",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Category Creation

    [Fact]
    public async Task AddCategory_WithValidData_ReturnsOkWithCategoryDTO()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var dto = new CreateCategoryDTO
        {
            Name = "Electronics",
            Code = "ELECTRONICS",
            Description = "Electronic products",
            ImageUrl = "electronics.jpg"
        };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PostAsync(
            "/api/categories",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var returnedCategory = await response.Content.ReadFromJsonAsync<CreateCategoryDTO>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(returnedCategory);
        Assert.Equal(dto.Name, returnedCategory.Name);
        Assert.Equal(dto.Code, returnedCategory.Code);
        Assert.Equal(dto.Description, returnedCategory.Description);
        Assert.Equal(dto.ImageUrl, returnedCategory.ImageUrl);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var savedCategory = await verificationContext.Categories
            .AsNoTracking()
            .SingleAsync(
                c => c.Code == dto.Code,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(dto.Name, savedCategory.Name);
        Assert.Equal(dto.Description, savedCategory.Description);
        Assert.Equal(dto.ImageUrl, savedCategory.ImageUrl);
    }

    [Fact]
    public async Task AddCategory_WithInvalidData_ReturnsBadRequestProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var dto = new CreateCategoryDTO
        {
            Name = "Electronics",
            Code = "electronics",
            Description = "Electronic products",
            ImageUrl = "electronics.jpg"
        };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PostAsync(
            "/api/categories",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.BadRequest,
            "Business rule violation");
    }

    #endregion

    #region Category Updates

    [Fact]
    public async Task UpdateCategory_WithValidData_ReturnsOkWithUpdatedCategoryDTO()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var category = CreateCategory(
            "Electronics",
            "ELECTRONICS",
            "Old description",
            "old.jpg");

        await SaveCategoriesAsync(category);

        var dto = new UpdateCategoryDTO
        {
            Name = "Computers",
            Description = "Computers and peripherals",
            ImageUrl = "computers.jpg"
        };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PutAsync(
            $"/api/categories/{category.Id}",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var returnedCategory = await response.Content.ReadFromJsonAsync<UpdateCategoryDTO>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(returnedCategory);
        Assert.Equal(dto.Name, returnedCategory.Name);
        Assert.Equal(dto.Description, returnedCategory.Description);
        Assert.Equal(dto.ImageUrl, returnedCategory.ImageUrl);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var savedCategory = await verificationContext.Categories
            .AsNoTracking()
            .SingleAsync(
                c => c.Id == category.Id,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(dto.Name, savedCategory.Name);
        Assert.Equal(dto.Description, savedCategory.Description);
        Assert.Equal(dto.ImageUrl, savedCategory.ImageUrl);

        // The update operation must preserve the existing category code.
        Assert.Equal("ELECTRONICS", savedCategory.Code);
    }

    [Fact]
    public async Task UpdateCategory_WhenCategoryDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var dto = new UpdateCategoryDTO
        {
            Name = "Computers",
            Description = "Computers and peripherals",
            ImageUrl = "computers.jpg"
        };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PutAsync(
            "/api/categories/2147483647",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    [Fact]
    public async Task UpdateCategory_WithInvalidData_ReturnsBadRequestProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var category = CreateCategory("Electronics", "ELECTRONICS");
        await SaveCategoriesAsync(category);

        var dto = new UpdateCategoryDTO
        {
            Name = "",
            Description = "Updated description",
            ImageUrl = "updated.jpg"
        };

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();
        using var content = JsonContent.Create(dto);

        // Act
        using var response = await client.PutAsync(
            $"/api/categories/{category.Id}",
            content,
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.BadRequest,
            "Business rule violation");
    }

    #endregion

    #region Category Deletion

    [Fact]
    public async Task DeleteCategory_WhenCategoryExists_ReturnsOk()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        var category = CreateCategory("Electronics", "ELECTRONICS");
        await SaveCategoriesAsync(category);

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.DeleteAsync(
            $"/api/categories/{category.Id}",
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        await using AppDbContext verificationContext = _databaseFixture.CreateContext();

        var deletedCategory = await verificationContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.Id == category.Id,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Null(deletedCategory);
    }

    [Fact]
    public async Task DeleteCategory_WhenCategoryDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        // Arrange
        await _databaseFixture.ResetDatabaseAsync();

        using var factory = new CustomWebApplicationFactory(_databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.DeleteAsync(
            "/api/categories/2147483647",
            TestContext.Current.CancellationToken);

        // Assert
        await AssertProblemDetailsAsync(
            response,
            HttpStatusCode.NotFound,
            "Resource not found");
    }

    #endregion

    #region Test Helpers

    // Creates a category with default test values and optional description and image.
    // Uses the same domain constructor as the application.
    private static Category CreateCategory(
        string name,
        string code,
        string? description = "Test description",
        string? imageUrl = "category.jpg")
    {
        return new Category(
            name,
            code,
            description,
            imageUrl);
    }

    // Saves categories using a separate DbContext to persist test data.
    // This allows API requests to retrieve categories that already exist in the database.
    private async Task SaveCategoriesAsync(params Category[] categories)
    {
        await using AppDbContext context = _databaseFixture.CreateContext();

        await context.Categories.AddRangeAsync(
            categories,
            TestContext.Current.CancellationToken);

        await context.SaveChangesAsync(
            TestContext.Current.CancellationToken);
    }

    // Verifies the HTTP status code and the main properties of a ProblemDetails response.
    // Centralizes error response assertions to avoid repeating the same checks in multiple tests.
    private static async Task AssertProblemDetailsAsync(HttpResponseMessage response, HttpStatusCode expectedStatus, string expectedTitle)
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
