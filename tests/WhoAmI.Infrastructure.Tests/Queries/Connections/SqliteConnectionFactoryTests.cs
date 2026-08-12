using System.Data;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using WhoAmI.Infrastructure.Data.Queries.Connections;

namespace WhoAmI.Infrastructure.Tests.Queries.Connections;

public sealed class SqliteConnectionFactoryTests {
    [Fact]
    public async Task
        CreateOpenConnectionAsync_ShouldReturnOpenConnection() {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?> {
                    ["ConnectionStrings:DefaultConnection"] =
                        "Data Source=:memory:"
                }
            )
            .Build();

        var sut = new SqliteConnectionFactory(configuration);

        // Act
        var result = await sut.CreateOpenConnectionAsync(
            TestContext.Current.CancellationToken
        );

        // Assert
        result.Should().BeOfType<SqliteConnection>();

        result.State.Should().Be(ConnectionState.Open);

        await result.DisposeAsync();
    }
}
