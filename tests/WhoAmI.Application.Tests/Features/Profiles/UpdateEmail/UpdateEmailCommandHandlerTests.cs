using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Time;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.UpdateEmail;
using WhoAmI.Domain.Profiles;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Application.Tests.Features.Profiles.UpdateEmail;

public class UpdateEmailCommandHandlerTests {
    [Fact]
    public async Task UpdateEmail_ShouldUpdateEmail_WhenNewEmailIsUnique() {
        // Arrange
        var profile = ProfileFactory.Create();

        var newEmail = "new.email@provider.com";

        var repo = Substitute.For<IProfileRepository>();

        var updatedAt = new DateTimeOffset(
            2026, 9, 23,
            10, 30, 0,
            TimeSpan.Zero
        );

        var clock = Substitute.For<IClock>();
        clock.UtcNow.Returns(updatedAt);

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(profile);

        var handler = new UpdateEmailCommandHandler(repo, clock);

        var command = new UpdateEmailCommand(profile.Id, newEmail);

        // Act
        var result = await handler.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        profile.Email.Address.Should().Be(newEmail);
        profile.UpdatedAt.Should().Be(updatedAt);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateEmail_ShouldReturnFailure_WhenProfileDoesNotExist() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        var clock = Substitute.For<IClock>();

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Profile?)null);

        var handler = new UpdateEmailCommandHandler(repo, clock);

        var id = Guid.NewGuid();

        var command = new UpdateEmailCommand(id, "new.email@provider.com");

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
    public async Task UpdateEmail_ShouldReturnFailure_WhenProfileIsAlreadyDeleted() {
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

        var currentEmail = profile.Email;

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(profile);

        var handler = new UpdateEmailCommandHandler(repo, clock);

        var command = new UpdateEmailCommand(
            profile.Id,
            "new.email@provider.com"
        );

        // Act
        var result = await handler.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        profile.Email.Should().Be(currentEmail);
        profile.UpdatedAt.Should().BeNull();

        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.AlreadyDeleted");
        result.FirstError.Metadata!["ProfileId"].Should().Be(profile.Id);
    }
}
