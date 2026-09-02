using FluentAssertions;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Testing;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Tests.Parsing.Commands.Profiles;

public sealed class CreateProfileParsingTests {
    private const string FirstName = "John";
    private const string LastName = "Doe";
    private const string Email = "john@doe.com";
    private const string LinkedIn = "https://www.linkedin.com/in/username";
    private const string GitHub = "https://github.com/username";

    private static readonly CommandAppTester _tester = CreateTester();

    private static CommandAppTester CreateTester() {
        var tester = new CommandAppTester();

        tester.Configure(c => {
            c.AddBranch("profile", profile => {
                profile.AddCommand<CreateProfile>("create");
            });
        });

        return tester;
    }

    private static CommandAppResult Parse(params string[] args) =>
        _tester.Run(["profile", "create", .. args]);

    private static CreateProfileCommandSettings GetSettings(
        CommandAppResult execution
    ) =>
        execution.Settings
            .Should()
            .BeOfType<CreateProfileCommandSettings>()
            .Subject;

    [Fact]
    public void CreateProfile_Should_ParseAllArguments() {
        // Act
        var execution = Parse(
            "--first-name", FirstName,
            "--last-name", LastName,
            "--email", Email,
            "--linkedin", LinkedIn,
            "--github", GitHub
        );

        // Assert
        var settings = GetSettings(execution);

        settings.FirstName.Should().Be(FirstName);
        settings.LastName.Should().Be(LastName);
        settings.Email.Should().Be(Email);
        settings.LinkedIn.Should().Be(LinkedIn);
        settings.GitHub.Should().Be(GitHub);
    }

    [Fact]
    public void CreateProfile_Should_ParseShortOptionsTogether() {
        // Act
        var execution = Parse(
            "-f", FirstName,
            "-l", LastName,
            "-e", Email,
            "-n", LinkedIn,
            "-g", GitHub
        );

        // Assert
        var settings = GetSettings(execution);

        settings.FirstName.Should().Be(FirstName);
        settings.LastName.Should().Be(LastName);
        settings.Email.Should().Be(Email);
        settings.LinkedIn.Should().Be(LinkedIn);
        settings.GitHub.Should().Be(GitHub);
    }

    [Theory]
    [InlineData("--first-name")]
    [InlineData("-f")]
    public void CreateProfile_Should_ParseFirstName(string option) {
        // Act
        var execution = Parse(option, FirstName);

        // Assert
        var settings = GetSettings(execution);

        settings.FirstName.Should().Be(FirstName);
    }

    [Theory]
    [InlineData("--last-name")]
    [InlineData("-l")]
    public void CreateProfile_Should_ParseLastName(string option) {
        // Act
        var execution = Parse(option, LastName);

        // Assert
        var settings = GetSettings(execution);

        settings.LastName.Should().Be(LastName);
    }

    [Theory]
    [InlineData("--email")]
    [InlineData("-e")]
    public void CreateProfile_Should_ParseEmail(string option) {
        // Act
        var execution = Parse(option, Email);

        // Assert
        var settings = GetSettings(execution);

        settings.Email.Should().Be(Email);
    }

    [Theory]
    [InlineData("--linkedin")]
    [InlineData("-n")]
    public void CreateProfile_Should_ParseLinkedIn(string option) {
        // Act
        var execution = Parse(option, LinkedIn);

        // Assert
        var settings = GetSettings(execution);

        settings.LinkedIn.Should().Be(LinkedIn);
    }

    [Theory]
    [InlineData("--github")]
    [InlineData("-g")]
    public void CreateProfile_Should_ParseGitHub(string option) {
        // Act
        var execution = Parse(option, GitHub);

        // Assert
        var settings = GetSettings(execution);

        settings.GitHub.Should().Be(GitHub);
    }
}
