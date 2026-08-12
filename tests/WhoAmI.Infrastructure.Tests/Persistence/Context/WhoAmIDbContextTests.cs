using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using WhoAmI.Application.Results;
using WhoAmI.Infrastructure.Data.Persistence.Context;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;
using WhoAmI.Infrastructure.Tests.Persistence.TestInfrastructure;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Infrastructure.Tests.Persistence.Context;

public class WhoAmIDbContextTests {
    [Fact]
    public async Task CommitAsync_ShouldReturnSuccess_WhenSaveSucceeds() {
        // Arrange
        var mapper = Substitute.For<IPersistenceErrorMapper>();

        await using var scope = new SqliteContextScope<WhoAmIDbContext>(
            options => new WhoAmIDbContext(options, mapper)
        );

        var context = scope.Context;

        context.Profiles.Add(ProfileFactory.Create());

        // Act
        var result = await context.CommitAsync(
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task CommitAsync_ShouldReturnFailure_WhenDbUpdateExceptionOccurs() {
        // Arrange
        var expected = new Error(
            "Profile.DuplicateEmail",
            "Email already exists."
        );

        var mapper = Substitute.For<IPersistenceErrorMapper>();

        mapper.Map(Arg.Any<DbUpdateException>()).Returns(expected);

        await using var scope = new SqliteContextScope<WhoAmIDbContext>(
            options => new WhoAmIDbContext(options, mapper)
        );

        var context = scope.Context;

        var profile1 = ProfileFactory.Create();
        var profile2 =  ProfileFactory.Create();

        context.Profiles.Add(profile1);
        await context.CommitAsync(TestContext.Current.CancellationToken);

        context.Profiles.Add(profile2);

        // Act
        var result = await context.CommitAsync(
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError.Should().Be(expected);
    }

    [Fact]
    public async Task CommitAsync_ShouldCallPersistenceErrorMapper_WhenExceptionOccurs() {
        // Arrange
        var mapper = Substitute.For<IPersistenceErrorMapper>();

        mapper.Map(Arg.Any<DbUpdateException>()).Returns(new Error("x", "y"));

        await using var scope = new SqliteContextScope<WhoAmIDbContext>(
            options => new WhoAmIDbContext(options, mapper)
        );

        var context = scope.Context;

        var profile1 = ProfileFactory.Create();
        var profile2 = ProfileFactory.Create();

        context.Profiles.Add(profile1);
        await context.CommitAsync(TestContext.Current.CancellationToken);

        context.Profiles.Add(profile2);

        // Act
        await context.CommitAsync(TestContext.Current.CancellationToken);

        // Assert
        mapper.Received(1).Map(Arg.Any<DbUpdateException>());
    }
}
