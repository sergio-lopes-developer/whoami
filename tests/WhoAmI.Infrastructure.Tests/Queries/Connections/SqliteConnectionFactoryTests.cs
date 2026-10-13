using System.Data;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using WhoAmI.Infrastructure.Data.Queries.Connections;

namespace WhoAmI.Infrastructure.Tests.Queries.Connections;

public class SqliteConnectionFactoryTests {
    [Fact]
    public async Task CreateOpenConnectionAsync_ShouldReturnOpenConnection() {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?> {
                    ["ConnectionStrings:DefaultConnection"] =
                        "Data Source=:memory:"
                }
            )
            .Build();

        var connectionFactory = new SqliteConnectionFactory(configuration);

        // Act
        var result = await connectionFactory.CreateOpenConnectionAsync(
            TestContext.Current.CancellationToken
        );

        // Assert
        result.Should().BeOfType<SqliteConnection>();
        result.State.Should().Be(ConnectionState.Open);

        await result.DisposeAsync();
    }

    [Fact]
    public void SqliteConnectionFactory_ShouldThrow_WhenDefaultConnectionStringIsMissing() {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act
        var act = () => new SqliteConnectionFactory(configuration);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                "The connection string 'DefaultConnection' was not configured."
            );
    }
}
