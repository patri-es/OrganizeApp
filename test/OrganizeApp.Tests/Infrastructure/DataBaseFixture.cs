
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Infrastructure.Persistence;

namespace OrganizeApp.Tests.Infrastructure;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private readonly DbContextOptions<AppDbContext> _options;

    public DatabaseFixture()
    {
        // Each fixture uses its own database.
        var databaseName = $"OrganizeApp_IntegrationTests_{Guid.NewGuid():N}";

        var connectionString =
            $"Server=(localdb)\\MSSQLLocalDB;" +
            $"Database={databaseName};" +
            "Integrated Security=True;" +
            "MultipleActiveResultSets=True;" +
            "TrustServerCertificate=True;" +
            "Encrypt=False";

        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;
    }

    // Creates a new DbContext instance.
    public AppDbContext CreateContext()
    {
        return new AppDbContext(_options);
    }

    // Initializes the test database using EF Core migrations.
    public async ValueTask InitializeAsync()
    {
        await using var context = CreateContext();

        await context.Database.MigrateAsync();
    }

    // Removes existing data before each test.
    public async Task ResetDatabaseAsync()
    {
        await using var context = CreateContext();

        await context.Database.ExecuteSqlRawAsync(
            "DELETE FROM [ProductCategories]; " +
            "DELETE FROM [Products]; " +
            "DELETE FROM [Categories];");
    }

    // Deletes the test database after the test class finishes.
    public async ValueTask DisposeAsync()
    {
        await using var context = CreateContext();

        await context.Database.EnsureDeletedAsync();
    }
}
