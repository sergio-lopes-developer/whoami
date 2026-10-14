using FluentAssertions;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Testing;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Tests.Parsing.Commands.Profiles;

public class GetProfileParsingTests {
    private const string Email = "john@doe.com";

    private static readonly CommandAppTester _tester = CreateTester();

    private static CommandAppTester CreateTester() {
        var tester = new CommandAppTester();

        tester.Configure(c => {
            c.AddBranch("profile", profile => {
                profile.AddCommand<GetProfile>("get");
            });
        });

        return tester;
    }

    private static CommandAppResult Parse(params string[] commandOptions) =>
        _tester.Run(["profile", "get", ..commandOptions]);

    private static GetProfileCommandSettings GetSettings(
        CommandAppResult execution
    ) =>
         execution.Settings
            .Should()
            .BeOfType<GetProfileCommandSettings>()
            .Subject;

    [Fact]
    public void GetProfile_ShouldParseAllOptions_WhenLongOptionsAreProvided() {
        // Act
        var execution = Parse(
            "--email", Email,
            "--hide-email",
            "--hide-github",
            "--hide-linkedin",
            "--verbose"
        );

        // Assert
        var settings = GetSettings(execution);

        settings.Email.Should().Be(Email);
        settings.HideEmail.Should().BeTrue();
        settings.HideGitHub.Should().BeTrue();
        settings.HideLinkedIn.Should().BeTrue();
        settings.Verbose.Should().BeTrue();
    }

    [Fact]
    public void GetProfile_ShouldParseAllOptions_WhenShortOptionsAreProvided() {
        // Act
        var execution = Parse(
            "-e", Email,
            "-m",
            "-g",
            "-n",
            "-v"
        );

        // Assert
        var settings = GetSettings(execution);

        settings.Email.Should().Be(Email);
        settings.HideEmail.Should().BeTrue();
        settings.HideGitHub.Should().BeTrue();
        settings.HideLinkedIn.Should().BeTrue();
        settings.Verbose.Should().BeTrue();
    }

    [Theory]
    [InlineData("--email")]
    [InlineData("-e")]
    public void GetProfile_ShouldParseEmail_WhenOptionIsProvided(
        string option
    ) {
        // Act
        var execution = Parse(option, Email);

        // Assert
        var settings = GetSettings(execution);

        settings.Email.Should().Be(Email);
    }

    [Theory]
    [InlineData("--hide-email")]
    [InlineData("-m")]
    public void GetProfile_ShouldParseHideEmail_WhenOptionIsProvided(
        string option
    ) {
        // Act
        var execution = Parse(option);

        // Assert
        var settings = GetSettings(execution);

        settings.HideEmail.Should().BeTrue();
    }

    [Theory]
    [InlineData("--hide-github")]
    [InlineData("-g")]
    public void GetProfile_ShouldParseHideGitHub_WhenOptionIsProvided(
        string option
    ) {
        // Act
        var execution = Parse(option);

        // Assert
        var settings = GetSettings(execution);

        settings.HideGitHub.Should().BeTrue();
    }

    [Theory]
    [InlineData("--hide-linkedin")]
    [InlineData("-n")]
    public void GetProfile_ShouldParseHideLinkedIn_WhenOptionIsProvided(
        string option
    ) {
        // Act
        var execution = Parse(option);

        // Assert
        var settings = GetSettings(execution);

        settings.HideLinkedIn.Should().BeTrue();
    }

    [Theory]
    [InlineData("--verbose")]
    [InlineData("-v")]
    public void GetProfile_ShouldParseVerbose_WhenOptionIsProvided(
        string option
    ) {
        // Act
        var execution = Parse(option);

        // Assert
        var settings = GetSettings(execution);

        settings.Verbose.Should().BeTrue();
    }

    [Fact]
    public void GetProfile_ShouldUseDefaultOptionValues_WhenOptionalOptionsAreNotProvided() {
        // Act
        var execution = Parse();

        // Assert
        var settings = GetSettings(execution);

        settings.HideEmail.Should().BeFalse();
        settings.HideGitHub.Should().BeFalse();
        settings.HideLinkedIn.Should().BeFalse();
        settings.Verbose.Should().BeFalse();
    }
}
