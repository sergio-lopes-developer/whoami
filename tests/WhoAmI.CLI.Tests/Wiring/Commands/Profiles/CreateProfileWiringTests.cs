using FluentAssertions;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.CLI.Output;
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
        // Act
        var command = RunCreateProfileCommand([
            "--first-name", "John",
            "--last-name", "Doe",
            "--email", "john@doe.com",
            "--linkedin", "linkedin",
            "--github", "github"
        ]);

        // Assert
        command.FirstName.Should().Be("John");
        command.LastName.Should().Be("Doe");
        command.Email.Should().Be("john@doe.com");
        command.LinkedIn.Should().Be("linkedin");
        command.GitHub.Should().Be("github");
    }

    [Theory]
    [InlineData("--first-name")]
    [InlineData("-f")]
    public void CreateProfile_Should_ParseFirstName(string option) {
        // Act
        var command = RunCreateProfileCommand([option, "John"]);

        // Assert
        command.FirstName.Should().Be("John");
    }

    [Theory]
    [InlineData("--last-name")]
    [InlineData("-l")]
    public void CreateProfile_Should_ParseLastName(string option) {
        // Act
        var command = RunCreateProfileCommand([option, "Doe"]);

        // Assert
        command.LastName.Should().Be("Doe");
    }

    [Theory]
    [InlineData("--email")]
    [InlineData("-e")]
    public void CreateProfile_Should_ParseEmail(string option) {
        // Act
        var command = RunCreateProfileCommand([option, "john@doe.com"]);

        // Assert
        command.Email.Should().Be("john@doe.com");
    }

    [Theory]
    [InlineData("--linkedin")]
    [InlineData("-n")]
    public void CreateProfile_Should_ParseLinkedin(string option) {
        // Act
        var command = RunCreateProfileCommand([option, "linkedin"]);

        // Assert
        command.LinkedIn.Should().Be("linkedin");
    }

    [Theory]
    [InlineData("--github")]
    [InlineData("-g")]
    public void CreateProfile_Should_ParseGithub(string option) {
        // Act
        var command = RunCreateProfileCommand([option, "github"]);

        // Assert
        command.GitHub.Should().Be("github");
    }
}
