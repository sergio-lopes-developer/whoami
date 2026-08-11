using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using WhoAmI.Infrastructure.Data.Persistence.Context;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;
using WhoAmI.Infrastructure.Data.Persistence.Repositories;
using WhoAmI.Infrastructure.Tests.Persistence.TestInfrastructure;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Infrastructure.Tests.Persistence.Repositories;

public sealed class ProfileRepositoryTests {
    [Fact]
    public async Task GetByIdAsync_ShouldReturnProfile_WhenProfileExists() {
        // Arrange
        var mapper = Substitute.For<IPersistenceErrorMapper>();

        await using var scope = new SqliteContextScope<WhoAmIDbContext>(
            options => new WhoAmIDbContext(options, mapper)
        );

        var context = scope.Context;

        var profile = ProfileFactory.Create();

        context.Profiles.Add(profile);

        await context.CommitAsync();

        var repository = new ProfileRepository(context);

        // Act
        var result = await repository.GetByIdAsync(profile.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(profile.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProfileDoesNotExist() {
        // Arrange
        var mapper = Substitute.For<IPersistenceErrorMapper>();

        await using var scope = new SqliteContextScope<WhoAmIDbContext>(
            options => new WhoAmIDbContext(options, mapper)
        );

        var context = scope.Context;

        var repository = new ProfileRepository(context);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Add_ShouldTrackProfileForInsertion() {
        // Arrange
        var mapper = Substitute.For<IPersistenceErrorMapper>();

        using var scope = new SqliteContextScope<WhoAmIDbContext>(
            options => new WhoAmIDbContext(options, mapper)
        );

        var context = scope.Context;

        var repository = new ProfileRepository(context);

        var profile = ProfileFactory.Create();

        // Act
        repository.Add(profile);

        // Assert
        var entry = context.Entry(profile);

        entry.State.Should().Be(EntityState.Added);
    }
}
