using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Time;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.UpdateFullName;
using WhoAmI.Domain.Profiles;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Application.Tests.Features.Profiles.UpdateFullName;

public class UpdateFullNameCommandHandlerTests {
    [Fact]
    public async Task UpdateFullName_ShouldUpdateFullName_WhenFirstAndLastNameAreValid() {
        // Arrange
        const string newFirstName = "John";
        const string newLastName = "Miller";
        var profile = ProfileFactory.Create();

        var updatedAt = new DateTimeOffset(
            2026, 9, 23,
            10, 30, 0,
            TimeSpan.Zero
        );

        var repo = Substitute.For<IProfileRepository>();

        var clock = Substitute.For<IClock>();
        clock.UtcNow.Returns(updatedAt);

        repo.GetByIdAsync(profile.Id, Arg.Any<CancellationToken>())
            .Returns(profile);

        var handler = new UpdateFullNameCommandHandler(repo, clock);

        var command = new UpdateFullNameCommand(
            profile.Id,
            newFirstName,
            newLastName
        );

        // Act
        var result = await handler.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        profile.FullName.FirstName.Value.Should().Be(newFirstName);
        profile.FullName.LastName.Value.Should().Be(newLastName);
        profile.UpdatedAt.Should().Be(updatedAt);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateFullName_ShouldReturnFailure_WhenProfileIsNotFound() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();
        var clock = Substitute.For<IClock>();

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Profile)null!);

        var handler = new UpdateFullNameCommandHandler(repo, clock);
        var id = Guid.NewGuid();
        var command = new UpdateFullNameCommand(id, "John", "Miller");

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
    public async Task UpdateFullName_ShouldReturnFailure_WhenProfileIsAlreadyDeleted() {
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

        var currentFirstName = profile.FullName.FirstName;
        var currentLastName = profile.FullName.LastName;

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(profile);

        var handler = new UpdateFullNameCommandHandler(repo, clock);
        var command = new UpdateFullNameCommand(profile.Id, "Changed", "Name");

        // Act
        var result = await handler.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        profile.FullName.FirstName.Should().Be(currentFirstName);
        profile.FullName.LastName.Should().Be(currentLastName);
        profile.UpdatedAt.Should().BeNull();

        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.AlreadyDeleted");
        result.FirstError.Metadata!["ProfileId"].Should().Be(profile.Id);
    }
}
