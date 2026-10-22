using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WhoAmI.Infrastructure.Data.Persistence.Context;
using WhoAmI.IntegrationTests.TestInfrastructure;

namespace WhoAmI.IntegrationTests.Bootstrap;

public class DatabaseInitializationTests : IClassFixture<TestApplicationFactory> {
    private readonly TestApplicationFactory _factory;

    public DatabaseInitializationTests(TestApplicationFactory factory) {
        _factory = factory;
    }

    [Fact]
    public async Task InitializeDatabaseAsync_ShouldCreateAccessibleMigratedDatabase() {
        // Arrange
        await using var scope = await _factory.CreateScopeAsync();

        var db = scope.Services.GetRequiredService<WhoAmIDbContext>();

        // Act
        var canConnect = await db.Database.CanConnectAsync(
            TestContext.Current.CancellationToken
        );

        var pending = await db.Database.GetPendingMigrationsAsync(
            TestContext.Current.CancellationToken
        );

        // Assert
        canConnect.Should().BeTrue();
        pending.Should().BeEmpty();
    }
}
