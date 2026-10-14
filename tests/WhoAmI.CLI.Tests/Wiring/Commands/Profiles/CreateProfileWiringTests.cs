using FluentAssertions;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public class CreateProfileWiringTests {
    private static CreateProfileResponse CreateProfileResponse() => new(
        Guid.NewGuid(),
        "John",
        "Doe",
        "john@doe.com",
        "https://www.linkedin.com/in/username",
        "https://github.com/username"
    );

    private static CreateProfileCommand RunCreateProfile(
        params string[] commandOptions
    ) {
        string[] commandLineArguments = ["profile", "create", ..commandOptions];

        return CliApplicationCommandRunner
            .Run<CreateProfileCommand, CreateProfileResponse>(
                CreateProfileResponse(),
                commandLineArguments
            );
    }

    [Fact]
    public void CreateProfile_ShouldBindOptionsAndDispatchCommand_WhenOptionsAreProvided() {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";
        const string email = "john@doe.com";
        const string linkedIn = "linkedin";
        const string gitHub = "github";

        // Act
        var command = RunCreateProfile([
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
}
