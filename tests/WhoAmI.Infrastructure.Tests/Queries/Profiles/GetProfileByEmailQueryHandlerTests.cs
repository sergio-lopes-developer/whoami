using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;
using WhoAmI.Infrastructure.Data.Queries.Profiles.GetProfileByEmail;
using WhoAmI.Infrastructure.Tests.Queries.TestDoubles;

namespace WhoAmI.Infrastructure.Tests.Queries.Profiles;

public sealed class GetProfileByEmailQueryHandlerTests {
    private static async Task CreateProfilesTable(SqliteConnection connection) {
        await connection.ExecuteAsync("""
            CREATE TABLE Profiles (
                id TEXT NOT NULL,
                first_name TEXT NOT NULL,
                last_name TEXT NOT NULL,
                email TEXT NOT NULL,
                linkedin_url TEXT NOT NULL,
                github_url TEXT NOT NULL
            );
        """);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnProfile_WhenProfileExists() {
        // Arrange
        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await CreateProfilesTable(connection);

        await connection.ExecuteAsync("""
            INSERT INTO Profiles (
                id,
                first_name,
                last_name,
                email,
                linkedin_url,
                github_url
            )
            VALUES (
                @Id,
                @FirstName,
                @LastName,
                @Email,
                @LinkedIn,
                @GitHub
            );
        """,
        new {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@doe.com",
            LinkedIn = "https://linkedin.com/john",
            GitHub = "https://github.com/john"
        });

        var connectionFactory = new FakeDbConnectionFactory(connection);

        var sut = new GetProfileByEmailQueryHandler(connectionFactory);

        var query = new GetProfileByEmailQuery("john@doe.com");

        // Act
        var result = await sut.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be("john@doe.com");
        result.Value.FirstName.Should().Be("John");
        result.Value.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenProfileDoesNotExist() {
        // Arrange
        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await CreateProfilesTable(connection);

        var connectionFactory = new FakeDbConnectionFactory(connection);

        var sut = new GetProfileByEmailQueryHandler(connectionFactory);

        var query = new GetProfileByEmailQuery("missing@profile.com");

        // Act
        var result = await sut.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
