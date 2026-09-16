using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using WhoAmI.Application.Features.Profiles.ListProfiles;
using WhoAmI.Infrastructure.Data.Queries.Features.Profiles.ListProfiles;
using WhoAmI.Infrastructure.Tests.Queries.TestDoubles;

namespace WhoAmI.Infrastructure.Tests.Queries.Features.Profiles;

public sealed class ListProfilesQueryHandlerTests {
    private static async Task CreateProfilesTable(SqliteConnection connection) {
        await connection.ExecuteAsync("""
            CREATE TABLE Profiles (
                first_name TEXT NOT NULL,
                last_name TEXT NOT NULL,
                email TEXT NOT NULL
            );
        """);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnProfilesOrderedByFirstNameThenLastName_WhenProfilesExist() {
        // Arrange
        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await CreateProfilesTable(connection);

        await connection.ExecuteAsync("""
            INSERT INTO Profiles (
                first_name,
                last_name,
                email
            )
            VALUES
                ('Zoe', 'Smith', 'zoe@test.com'),
                ('Alice', 'Brown', 'alice@test.com'),
                ('Alice', 'Anderson', 'alice2@test.com');
        """);

        var connectionFactory = new FakeDbConnectionFactory(connection);

        var sut = new ListProfilesQueryHandler(connectionFactory);

        // Act
        var result = await sut.HandleAsync(
            new ListProfilesQuery(),
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value
            .Should()
            .BeEquivalentTo(
                [
                    new ListProfilesResponse(
                        "Alice",
                        "Anderson",
                        "alice2@test.com"
                    ),
                    new ListProfilesResponse(
                        "Alice",
                        "Brown",
                        "alice@test.com"
                    ),
                    new ListProfilesResponse(
                        "Zoe",
                        "Smith",
                        "zoe@test.com"
                    )
                ],
                options => options.WithStrictOrdering()
            );
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnEmptyCollection_WhenNoProfilesExist() {
        // Arrange
        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await CreateProfilesTable(connection);

        var connectionFactory = new FakeDbConnectionFactory(connection);

        var sut = new ListProfilesQueryHandler(connectionFactory);

        // Act
        var result = await sut.HandleAsync(
            new ListProfilesQuery(),
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
