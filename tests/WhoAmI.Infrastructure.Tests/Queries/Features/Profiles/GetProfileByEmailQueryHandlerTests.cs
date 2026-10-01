using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;
using WhoAmI.Infrastructure.Data.Queries.Features.Profiles.GetProfileByEmail;
using WhoAmI.Infrastructure.Tests.Queries.TestDoubles;

namespace WhoAmI.Infrastructure.Tests.Queries.Features.Profiles;

public sealed class GetProfileByEmailQueryHandlerTests {
    private static async Task CreateProfilesTable(SqliteConnection connection) {
        await connection.ExecuteAsync(
            """
                CREATE TABLE Profiles (
                    id TEXT NOT NULL,
                    first_name TEXT NOT NULL,
                    last_name TEXT NOT NULL,
                    email TEXT NOT NULL,
                    linkedin_url TEXT NOT NULL,
                    github_url TEXT NOT NULL,
                    deleted_at TEXT NULL
                );
            """
        );
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnProfile_WhenProfileExists() {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";
        const string email = "john@doe.com";

        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await CreateProfilesTable(connection);

        await connection.ExecuteAsync(
            """
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
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                LinkedIn = "https://linkedin.com/john",
                GitHub = "https://github.com/john"
            }
        );

        var connectionFactory = new FakeDbConnectionFactory(connection);

        var sut = new GetProfileByEmailQueryHandler(connectionFactory);

        var query = new GetProfileByEmailQuery(email);

        // Act
        var result = await sut.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be(email);
        result.Value.FirstName.Should().Be(firstName);
        result.Value.LastName.Should().Be(lastName);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenProfileDoesNotExist() {
        // Arrange
        const string email = "missing@profile.com";

        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await CreateProfilesTable(connection);

        var connectionFactory = new FakeDbConnectionFactory(connection);

        var sut = new GetProfileByEmailQueryHandler(connectionFactory);

        var query = new GetProfileByEmailQuery(email);

        // Act
        var result = await sut.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.NotFoundByEmail");
        result.FirstError.Metadata!["ProfileEmail"].Should().Be(email);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenProfileIsDeleted() {
        // Arrange
        const string email = "john@doe.com";

        var deletedAt = new DateTimeOffset(
            2026, 9, 30,
            14, 15, 0,
            TimeSpan.FromHours(-3)
        );

        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await CreateProfilesTable(connection);

        await connection.ExecuteAsync(
            """
                INSERT INTO Profiles (
                    id,
                    first_name,
                    last_name,
                    email,
                    linkedin_url,
                    github_url,
                    deleted_at
                )
                VALUES (
                    @Id,
                    @FirstName,
                    @LastName,
                    @Email,
                    @LinkedIn,
                    @GitHub,
                    @DeletedAt
                );
            """,
            new {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = email,
                LinkedIn = "https://linkedin.com/john",
                GitHub = "https://github.com/john",
                DeletedAt = deletedAt
            }
        );

        var connectionFactory = new FakeDbConnectionFactory(connection);

        var sut = new GetProfileByEmailQueryHandler(connectionFactory);

        var query = new GetProfileByEmailQuery(email);

        // Act
        var result = await sut.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.NotFoundByEmail");
        result.FirstError.Metadata!["ProfileEmail"].Should().Be(email);
    }
}
