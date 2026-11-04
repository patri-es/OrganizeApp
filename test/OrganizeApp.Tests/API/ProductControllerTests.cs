
using System.Net;
using System.Net.Http.Json;
using OrganizeApp.Application.Product.DTOs;
using OrganizeApp.Tests.Infrastructure;

namespace OrganizeApp.Tests.API;

public class ProductControllerTests(DatabaseFixture databaseFixture) : IClassFixture<DatabaseFixture>
{
    [Fact]
    public async Task GetProducts_WhenNoProductsExist_ReturnsOkWithEmptyList()
    {
        // Arrange
        await databaseFixture.ResetDatabaseAsync();

        using var factory = new CustomWebApplicationFactory(databaseFixture.ConnectionString);

        using var client = factory.CreateClient();

        // Act
        using var response = await client.GetAsync("/api/products", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<ProductDTO>>(
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(products);
        Assert.Empty(products);
    }
}
