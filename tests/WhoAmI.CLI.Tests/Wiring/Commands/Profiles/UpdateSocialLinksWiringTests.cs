using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateSocialLinks;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public sealed class UpdateSocialLinksWiringTests {
    private static UpdateSocialLinksCommand RunUpdateSocialLinks(
        params string[] args
    ) {
        string[] commandArgs = ["profile", "update", "social-links", ..args];

        return CliApplicationCommandRunner
            .Run<UpdateSocialLinksCommand>(commandArgs);
    }

    [Fact]
    public void UpdateSocialLinks_Should_BindCliArgumentsAndDispatchCommand() {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const string linkedIn = "https://www.linkedin.com/in/username";
        const string gitHub = "https://github.com/username";

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
