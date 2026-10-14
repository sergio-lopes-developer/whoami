using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateSocialLinks;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public class UpdateSocialLinksWiringTests {
    private static UpdateSocialLinksCommand RunUpdateSocialLinks(
        params string[] commandOptions
    ) {
        string[] commandLineArguments = [
            "profile",
            "update",
            "social-links",
            ..commandOptions
        ];

        return CliApplicationCommandRunner
            .Run<UpdateSocialLinksCommand>(commandLineArguments);
    }

    [Fact]
    public void UpdateSocialLinks_ShouldBindOptionsAndDispatchCommand_WhenOptionsAreProvided() {
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
