using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateSocialLinks;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public sealed class UpdateSocialLinksWiringTests {
    private static UpdateSocialLinksCommand RunUpdateSocialLinksCommand(
        params string[] args
    ) {
        string[] commandArgs = ["profile", "update", "social-links", ..args];

        return CliCommandRunner.Run<UpdateSocialLinksCommand>(commandArgs);
    }

    [Fact]
    public void UpdateSocialLinks_Should_ParseAllArguments() {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const string linkedIn = "https://www.linkedin.com/in/username";
        const string gitHub = "https://github.com/username";

        // Act
        var command = RunUpdateSocialLinksCommand(
            "--id", id,
            "--linkedin", linkedIn,
            "--github", gitHub
        );

        // Assert
        command.Id.Should().Be(id);
        command.LinkedIn.Should().Be(linkedIn);
        command.GitHub.Should().Be(gitHub);
    }

    [Theory]
    [InlineData("--id")]
    [InlineData("-i")]
    public void UpdateSocialLinks_Should_ParseId(string option) {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        var command = RunUpdateSocialLinksCommand(option, id);

        // Assert
        command.Id.Should().Be(id);
    }

    [Theory]
    [InlineData("--linkedin")]
    [InlineData("-n")]
    public void UpdateSocialLinks_Should_ParseLinkedIn(string option) {
        // Arrange
        const string linkedIn = "https://www.linkedin.com/in/username";

        // Act
        var command = RunUpdateSocialLinksCommand(option, linkedIn);

        // Assert
        command.LinkedIn.Should().Be(linkedIn);
    }

    [Theory]
    [InlineData("--github")]
    [InlineData("-g")]
    public void UpdateSocialLinks_Should_ParseGitHub(string option) {
        // Arrange
        const string gitHub = "https://github.com/username";

        // Act
        var command = RunUpdateSocialLinksCommand(option, gitHub);

        // Assert
        command.GitHub.Should().Be(gitHub);
    }
}
