using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.Domain.Profiles;

namespace WhoAmI.Application.Tests.Features.Profiles.CreateProfile;

public class CreateProfileCommandHandlerTests {
    [Fact]
    public async Task CreateProfile_ShouldCreateProfile_WhenEmailIsUnique() {
        // Arrange
        var repo =  Substitute.For<IProfileRepository>();

        var handler = new CreateProfileCommandHandler(repo);

        var command = new CreateProfileCommand(
            "Sérgio",
            "Lopes",
            "email@provider.com",
            "https://www.linkedin.com/in/username",
            "https://github.com/username"
        );

        // Act
        var result = await handler.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        repo.Received(1).Add(
            Arg.Is<Profile>(p =>
                p != null &&
                p.Email.Address == command.Email &&
                p.FullName.FirstName.Value == command.FirstName &&
                p.FullName.LastName.Value == command.LastName &&
                p.LinkedIn.Value == command.LinkedIn &&
                p.GitHub.Value == command.GitHub
            )
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBe(Guid.Empty);
        result.Value.FirstName.Should().Be(command.FirstName);
        result.Value.LastName.Should().Be(command.LastName);
        result.Value.Email.Should().Be(command.Email);
        result.Value.LinkedIn.Should().Be(command.LinkedIn);
        result.Value.GitHub.Should().Be(command.GitHub);
    }
}
