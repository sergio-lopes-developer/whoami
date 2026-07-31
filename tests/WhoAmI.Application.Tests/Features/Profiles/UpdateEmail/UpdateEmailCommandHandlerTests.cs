using FluentAssertions;
using NSubstitute;
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

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(profile);

        var handler = new UpdateEmailCommandHandler(repo);

        var command = new UpdateEmailCommand(profile.Id, newEmail);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        profile.Email.Address.Should().Be(newEmail);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateEmail_ShouldReturnFailure_WhenProfileDoesNotExist() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        repo.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Profile?)null);

        var handler = new UpdateEmailCommandHandler(repo);

        var id = Guid.NewGuid();

        var command = new UpdateEmailCommand(id, "new.email@provider.com");

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.NotFound");
        result.FirstError.Metadata!["ProfileId"].Should().Be(id);
    }
}
