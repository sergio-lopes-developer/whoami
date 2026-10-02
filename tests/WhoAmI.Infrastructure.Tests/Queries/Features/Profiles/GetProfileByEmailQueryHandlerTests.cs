using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;
using WhoAmI.Infrastructure.Data.Queries.Features.Profiles.GetProfileByEmail;
using WhoAmI.Infrastructure.Tests.Queries.TestDoubles;

namespace WhoAmI.Infrastructure.Tests.Queries.Features.Profiles;

public sealed class GetProfileByEmailQueryHandlerTests {
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
    public async Task HandleAsync_ShouldReturnProfile_WhenProfileExists() {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";
        const string email = "john@doe.com";
        const string linkedIn = "https://linkedin.com/john";
        const string gitHub = "https://github.com/john";

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
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                LinkedIn = linkedIn,
                GitHub = gitHub,
                CreatedAt = _createdAt,
                UpdatedAt = updatedAt
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
        result.Value.LinkedIn.Should().Be(linkedIn);
        result.Value.GitHub.Should().Be(gitHub);
        result.Value.CreatedAt.Should().Be(_createdAt);
        result.Value.UpdatedAt.Should().Be(updatedAt);
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
                FirstName = "John",
                LastName = "Doe",
                Email = email,
                LinkedIn = "https://linkedin.com/john",
                GitHub = "https://github.com/john",
                CreatedAt = _createdAt,
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

    [Fact]
    public async Task HandleAsync_ShouldReturnProfileWithoutUpdatedAt_WhenProfileHasNeverBeenUpdated() {
        // Arrange
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
                FirstName = "John",
                LastName = "Doe",
                Email = email,
                LinkedIn = "https://linkedin.com/john",
                GitHub = "https://github.com/john",
                CreatedAt = _createdAt
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
        result.Value.CreatedAt.Should().Be(_createdAt);
        result.Value.UpdatedAt.Should().BeNull();
    }
}
