
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Domain.Entities;
using OrganizeApp.Infrastructure.Persistence;
using OrganizeApp.Infrastructure.Persistence.Repositories;

namespace OrganizeApp.Tests.Infrastructure;

public class CategoryRepositoryTests(DatabaseFixture fixture)
    : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture = fixture;

    #region Category Queries

    [Fact]
    public async Task GetCategories_WithCategories_ReturnsAllCategories()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var categories = new[]
            {
                CreateCategory("Electronics", "ELECTRONICS"),
                CreateCategory("Home", "HOME")
            };

            await AddCategoriesAsync(arrangeContext, categories);
        }

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new CategoryRepository(context);

        var result = await repository.GetCategoriesAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, category => category.Name == "Electronics");
        Assert.Contains(result, category => category.Name == "Home");
    }

    [Fact]
    public async Task GetCategories_WhenNoCategoriesExist_ReturnsEmptyList()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using var context = _fixture.CreateContext();
        var repository = new CategoryRepository(context);

        // Act
        var result = await repository.GetCategoriesAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetById_WhenCategoryExists_ReturnsCategory()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        int categoryId;

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var category = CreateCategory(
                "Electronics",
                "ELECTRONICS");

            await AddCategoriesAsync(arrangeContext, category);

            categoryId = category.Id;
        }

        // Act
        await using var context = _fixture.CreateContext();
        var repository = new CategoryRepository(context);

        var result = await repository.GetByIdAsync(categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoryId, result.Id);
        Assert.Equal("Electronics", result.Name);
        Assert.Equal("ELECTRONICS", result.Code);
        Assert.Equal("Test description", result.Description);
        Assert.Equal("category.jpg", result.ImageUrl);
    }

    [Fact]
    public async Task GetById_WhenCategoryDoesNotExist_ReturnsNull()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await using var context = _fixture.CreateContext();
        var repository = new CategoryRepository(context);

        // Act
        var result = await repository.GetByIdAsync(int.MaxValue);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region Category Persistence

    [Fact]
    public async Task AddAsync_AndSaveChangesAsync_WithValidCategory_PersistsCategory()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        var category = CreateCategory(
            "Electronics",
            "ELECTRONICS");

        await using (var context = _fixture.CreateContext())
        {
            var repository = new CategoryRepository(context);

            // Act
            await repository.AddAsync(category);
            await repository.SaveChangesAsync();
        }

        // Assert
        await using AppDbContext verificationContext = _fixture.CreateContext();

        var savedCategory = await verificationContext.Categories
            .AsNoTracking()
            .SingleAsync(
                c => c.Id == category.Id,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("Electronics", savedCategory.Name);
        Assert.Equal("ELECTRONICS", savedCategory.Code);
        Assert.Equal("Test description", savedCategory.Description);
        Assert.Equal("category.jpg", savedCategory.ImageUrl);
    }

    [Fact]
    public async Task SaveChangesAsync_WithModifiedCategory_PersistsChanges()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        var category = CreateCategory(
            "Electronics",
            "ELECTRONICS");

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var repository = new CategoryRepository(arrangeContext);

            await repository.AddAsync(category);
            await repository.SaveChangesAsync();
        }

        int categoryId = category.Id;

        // Act
        await using (var context = _fixture.CreateContext())
        {
            var repository = new CategoryRepository(context);

            var existingCategory = await repository.GetByIdAsync(categoryId);

            Assert.NotNull(existingCategory);

            existingCategory.ChangeName("Computers");
            existingCategory.ChangeCode("COMPUTERS");
            existingCategory.ChangeDescription("Computers and peripherals");
            existingCategory.ChangeImage("computers.jpg");

            await repository.SaveChangesAsync();
        }

        // Assert
        await using AppDbContext verificationContext = _fixture.CreateContext();

        var savedCategory = await verificationContext.Categories
            .AsNoTracking()
            .SingleAsync(
                c => c.Id == categoryId,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("Computers", savedCategory.Name);
        Assert.Equal("COMPUTERS", savedCategory.Code);
        Assert.Equal("Computers and peripherals", savedCategory.Description);
        Assert.Equal("computers.jpg", savedCategory.ImageUrl);
    }

    [Fact]
    public async Task DeleteAsync_AndSaveChangesAsync_WithExistingCategory_DeletesCategory()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        int categoryId;

        await using (var arrangeContext = _fixture.CreateContext())
        {
            var category = CreateCategory(
                "Electronics",
                "ELECTRONICS");

            var repository = new CategoryRepository(arrangeContext);

            await repository.AddAsync(category);
            await repository.SaveChangesAsync();

            categoryId = category.Id;
        }

        // Act
        await using (var context = _fixture.CreateContext())
        {
            var repository = new CategoryRepository(context);

            var category = await repository.GetByIdAsync(categoryId);

            Assert.NotNull(category);

            await repository.DeleteAsync(category);
            await repository.SaveChangesAsync();
        }

        // Assert
        await using AppDbContext verificationContext = _fixture.CreateContext();

        var deletedCategory = await verificationContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(
                c => c.Id == categoryId,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Null(deletedCategory);
    }

    #endregion

    #region Test Helpers

    private static Category CreateCategory(string name, string code)
    {
        return new Category(
            name,
            code,
            "Test description",
            "category.jpg");
    }

    private static async Task AddCategoriesAsync(
        AppDbContext context,
        params Category[] categories)
    {
        await context.Categories.AddRangeAsync(
            categories,
            TestContext.Current.CancellationToken);

        await context.SaveChangesAsync(
            TestContext.Current.CancellationToken);
    }

    #endregion


}
