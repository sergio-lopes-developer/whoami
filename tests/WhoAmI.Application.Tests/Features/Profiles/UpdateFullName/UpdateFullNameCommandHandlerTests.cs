using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.UpdateFullName;
using WhoAmI.Domain.Profiles;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Application.Tests.Features.Profiles.UpdateFullName;

public class UpdateFullNameCommandHandlerTests {
    [Fact]
    public async Task UpdateFullName_ShouldUpdateFullName_WhenFirstAndLastNameAreValid() {
        // Arrange
        var profile = ProfileFactory.Create();

        var newFirstName = "John";

        var newLastName = "Miller";

        var repo = Substitute.For<IProfileRepository>();

        repo.GetByIdAsync(profile.Id, Arg.Any<CancellationToken>())
            .Returns(profile);

        var handler = new UpdateFullNameCommandHandler(repo);

        var command = new UpdateFullNameCommand(
            profile.Id,
            newFirstName,
            newLastName
        );

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        profile.FullName.FirstName.Value.Should().Be(newFirstName);
        profile.FullName.LastName.Value.Should().Be(newLastName);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateFullName_ShouldReturnFailure_WhenProfileIsNotFound() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Profile)null!);

        var handler = new UpdateFullNameCommandHandler(repo);

        var id = Guid.NewGuid();

        var command = new UpdateFullNameCommand(id, "John", "Miller");

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.NotFound");
        result.FirstError.Metadata!["ProfileId"].Should().Be(id);
    }
}
