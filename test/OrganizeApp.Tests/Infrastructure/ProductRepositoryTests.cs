using OrganizeApp.Infrastructure.Persistence.Repositories;

namespace OrganizeApp.Tests.Infrastructure;

public class ProductRepositoryTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture = fixture;

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
}