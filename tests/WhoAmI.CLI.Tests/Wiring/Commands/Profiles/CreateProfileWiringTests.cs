using FluentAssertions;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public sealed class CreateProfileWiringTests {
    private static CreateProfileResponse CreateProfileResponse() => new(
        Guid.NewGuid(),
        "John",
        "Doe",
        "john@doe.com",
        "linkedin",
        "github"
    );

    private static CreateProfileCommand RunCreateProfileCommand(
        params string[] args
    ) {
        string[] commandArgs = ["profile", "create", ..args];

        return
            CliCommandRunner.Run<CreateProfileCommand, CreateProfileResponse>(
                CreateProfileResponse(),
                commandArgs
            );
    }

    [Fact]
    public void CreateProfile_Should_ParseAllArguments() {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";
        const string email = "john@doe.com";
        const string linkedIn = "linkedin";
        const string gitHub = "github";

        // Act
        var command = RunCreateProfileCommand([
            "--first-name", firstName,
            "--last-name", lastName,
            "--email", email,
            "--linkedin", linkedIn,
            "--github", gitHub
        ]);

        // Assert
        command.FirstName.Should().Be(firstName);
        command.LastName.Should().Be(lastName);
        command.Email.Should().Be(email);
        command.LinkedIn.Should().Be(linkedIn);
        command.GitHub.Should().Be(gitHub);
    }

    [Theory]
    [InlineData("--first-name")]
    [InlineData("-f")]
    public void CreateProfile_Should_ParseFirstName(string option) {
        // Arrange
        const string firstName = "John";

        // Act
        var command = RunCreateProfileCommand(option, firstName);

        // Assert
        command.FirstName.Should().Be(firstName);
    }

    [Theory]
    [InlineData("--last-name")]
    [InlineData("-l")]
    public void CreateProfile_Should_ParseLastName(string option) {
        // Arrange
        const string lastName = "Doe";

        // Act
        var command = RunCreateProfileCommand(option, lastName);

        // Assert
        command.LastName.Should().Be(lastName);
    }

    [Theory]
    [InlineData("--email")]
    [InlineData("-e")]
    public void CreateProfile_Should_ParseEmail(string option) {
        // Arrange
        const string email = "john@doe.com";

        // Act
        var command = RunCreateProfileCommand(option, email);

        // Assert
        command.Email.Should().Be(email);
    }

    [Theory]
    [InlineData("--linkedin")]
    [InlineData("-n")]
    public void CreateProfile_Should_ParseLinkedin(string option) {
        // Arrange
        const string linkedIn = "linkedin";

        // Act
        var command = RunCreateProfileCommand(option, linkedIn);

        // Assert
        command.LinkedIn.Should().Be(linkedIn);
    }

    [Theory]
    [InlineData("--github")]
    [InlineData("-g")]
    public void CreateProfile_Should_ParseGithub(string option) {
        // Arrange
        const string gitHub = "github";

        // Act
        var command = RunCreateProfileCommand(option, gitHub);

        // Assert
        command.GitHub.Should().Be(gitHub);
    }
}
