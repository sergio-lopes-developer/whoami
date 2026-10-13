using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;
using WhoAmI.Infrastructure.Data.Queries.Features.Profiles.GetProfileByEmail;
using WhoAmI.Infrastructure.Tests.Queries.TestDoubles;

namespace WhoAmI.Infrastructure.Tests.Queries.Features.Profiles;

public class GetProfileByEmailQueryHandlerTests {
    private const string FirstName = "John";
    private const string LastName = "Doe";
    private const string Email = "john@doe.com";
    private const string LinkedIn = "https://linkedin.com/john";
    private const string GitHub = "https://github.com/john";

    private static readonly DateTimeOffset _createdAt =
        new(2026, 9, 22, 13, 25, 0, TimeSpan.Zero);

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
                    created_at TEXT NOT NULL,
                    updated_at TEXT NULL,
                    deleted_at TEXT NULL
                );
            """
        );
    }

    [Fact]
    public async Task GetProfileByEmail_ShouldReturnProfile_WhenProfileExists() {
        // Arrange
        var updatedAt = new DateTimeOffset(
            2026, 9, 23,
            10, 15, 0,
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
                    created_at,
                    updated_at
                )
                VALUES (
                    @Id,
                    @FirstName,
                    @LastName,
                    @Email,
                    @LinkedIn,
                    @GitHub,
                    @CreatedAt,
                    @UpdatedAt
                );
            """,
            new {
                Id = Guid.NewGuid(),
                FirstName,
                LastName,
                Email,
                LinkedIn,
                GitHub,
                CreatedAt = _createdAt,
                UpdatedAt = updatedAt
            }
        );

        var connectionFactory = new FakeDbConnectionFactory(connection);
        var handler = new GetProfileByEmailQueryHandler(connectionFactory);
        var query = new GetProfileByEmailQuery(Email);

        // Act
        var result = await handler.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsSuccess.Should().BeTrue();

        var value = result.GetValueOrThrow();

        value.Email.Should().Be(Email);
        value.FirstName.Should().Be(FirstName);
        value.LastName.Should().Be(LastName);
        value.LinkedIn.Should().Be(LinkedIn);
        value.GitHub.Should().Be(GitHub);
        value.CreatedAt.Should().Be(_createdAt);
        value.UpdatedAt.Should().Be(updatedAt);
    }

    [Fact]
    public async Task GetProfileByEmail_ShouldReturnFailure_WhenProfileDoesNotExist() {
        // Arrange
        const string email = "missing@profile.com";

        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync(TestContext.Current.CancellationToken);

        await CreateProfilesTable(connection);

        var connectionFactory = new FakeDbConnectionFactory(connection);
        var handler = new GetProfileByEmailQueryHandler(connectionFactory);
        var query = new GetProfileByEmailQuery(email);

        // Act
        var result = await handler.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.NotFoundByEmail");
        result.FirstError.Metadata!["ProfileEmail"].Should().Be(email);
    }

    [Fact]
    public async Task GetProfileByEmail_ShouldReturnFailure_WhenProfileIsDeleted() {
        // Arrange
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
                    created_at,
                    deleted_at
                )
                VALUES (
                    @Id,
                    @FirstName,
                    @LastName,
                    @Email,
                    @LinkedIn,
                    @GitHub,
                    @CreatedAt,
                    @DeletedAt
                );
            """,
            new {
                Id = Guid.NewGuid(),
                FirstName,
                LastName,
                Email,
                LinkedIn,
                GitHub,
                CreatedAt = _createdAt,
                DeletedAt = deletedAt
            }
        );

        var connectionFactory = new FakeDbConnectionFactory(connection);
        var handler = new GetProfileByEmailQueryHandler(connectionFactory);
        var query = new GetProfileByEmailQuery(Email);

        // Act
        var result = await handler.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.NotFoundByEmail");
        result.FirstError.Metadata!["ProfileEmail"].Should().Be(Email);
    }

    [Fact]
    public async Task GetProfileByEmail_ShouldReturnProfile_WhenProfileHasNeverBeenUpdated() {
        // Arrange
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
                    created_at
                )
                VALUES (
                    @Id,
                    @FirstName,
                    @LastName,
                    @Email,
                    @LinkedIn,
                    @GitHub,
                    @CreatedAt
                );
            """,
            new {
                Id = Guid.NewGuid(),
                FirstName,
                LastName,
                Email,
                LinkedIn,
                GitHub,
                CreatedAt = _createdAt
            }
        );

        var connectionFactory = new FakeDbConnectionFactory(connection);
        var handler = new GetProfileByEmailQueryHandler(connectionFactory);
        var query = new GetProfileByEmailQuery(Email);

        // Act
        var result = await handler.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsSuccess.Should().BeTrue();

        var value = result.GetValueOrThrow();

        value.CreatedAt.Should().Be(_createdAt);
        value.UpdatedAt.Should().BeNull();
    }
}
