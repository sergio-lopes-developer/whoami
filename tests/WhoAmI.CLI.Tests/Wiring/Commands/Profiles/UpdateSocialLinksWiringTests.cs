using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateSocialLinks;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public class UpdateSocialLinksWiringTests {
    private static UpdateSocialLinksCommand RunUpdateSocialLinks(
        params string[] args
    ) {
        string[] commandArgs = ["profile", "update", "social-links", ..args];

        return CliApplicationCommandRunner
            .Run<UpdateSocialLinksCommand>(commandArgs);
    }

    [Fact]
    public void UpdateSocialLinks_ShouldBindCliArgumentsAndDispatchCommand_WhenArgumentsAreProvided() {
        // Arrange
        const string linkedIn = "https://www.linkedin.com/in/username";
        const string gitHub = "https://github.com/username";
        var id = Guid.NewGuid().ToString();

        // Act
        var command = RunUpdateSocialLinks(
            "--id", id,
            "--linkedin", linkedIn,
            "--github", gitHub
        );

        // Assert
        command.Id.Should().Be(id);
        command.LinkedIn.Should().Be(linkedIn);
        command.GitHub.Should().Be(gitHub);
    }
}
