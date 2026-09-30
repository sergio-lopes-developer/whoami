using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Time;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.DeleteProfile;
using WhoAmI.Domain.Profiles;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Application.Tests.Features.Profiles.DeleteProfile;

public class DeleteProfileCommandHandlerTests {
    [Fact]
    public async Task DeleteProfile_ShouldDeleteProfile_WhenProfileExists() {
        // Arrange
        var profile = ProfileFactory.Create();

        var repo = Substitute.For<IProfileRepository>();

        var deletedAt = new DateTimeOffset(
            2026, 9, 29,
            18, 40, 0,
            TimeSpan.Zero
        );

        var clock = Substitute.For<IClock>();
        clock.UtcNow.Returns(deletedAt);

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(profile);

        var handler = new DeleteProfileCommandHandler(repo, clock);

        var command = new DeleteProfileCommand(profile.Id);

        // Act
        var result = await handler.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        await repo.Received(1)
            .GetByIdAsync(profile.Id, Arg.Any<CancellationToken>());

        profile.IsDeleted.Should().BeTrue();
        profile.DeletedAt.Should().Be(deletedAt);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteProfile_ShouldReturnFailure_WhenProfileDoesNotExist() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        var clock = Substitute.For<IClock>();

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Profile?)null);

        var handler = new DeleteProfileCommandHandler(repo, clock);

        var id = Guid.NewGuid();

        var command = new DeleteProfileCommand(id);

        // Act
        var result = await handler.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.NotFound");
        result.FirstError.Metadata!["ProfileId"].Should().Be(id);
    }

    [Fact]
    public async Task DeleteProfile_ShouldReturnFailure_WhenProfileIsAlreadyDeleted() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        var clock = Substitute.For<IClock>();

        var deletedAt = new DateTimeOffset(
            2026, 9, 29,
            11, 30, 0,
            TimeSpan.FromHours(-3)
        );

        var profile = ProfileFactory.CreateWithoutEvents();
        profile.Delete(deletedAt);

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(profile);

        var handler = new DeleteProfileCommandHandler(repo, clock);

        var command = new DeleteProfileCommand(profile.Id);

        // Act
        var result = await handler.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.AlreadyDeleted");
        result.FirstError.Metadata!["ProfileId"].Should().Be(profile.Id);
    }
}
