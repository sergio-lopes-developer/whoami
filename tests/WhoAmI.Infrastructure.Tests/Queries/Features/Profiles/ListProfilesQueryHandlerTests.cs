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
    public async Task
        HandleAsync_ShouldReturnProfilesOrderedByFirstNameThenLastName() {
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
        result.Value.Select(x => x.FullName)
            .Should()
            .ContainInOrder(
                "Alice Anderson",
                "Alice Brown",
                "Zoe Smith"
            );
    }
}
