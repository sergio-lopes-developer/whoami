using FluentAssertions;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Testing;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Tests.Parsing.Commands.Profiles;

public sealed class GetProfileParsingTests {
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

    private static CommandAppResult Parse(params string[] args) =>
        _tester.Run(["profile", "get", .. args]);

    private static GetProfileCommandSettings GetSettings(
        CommandAppResult execution
    ) =>
         execution.Settings
            .Should()
            .BeOfType<GetProfileCommandSettings>()
            .Subject;

    [Fact]
    public void GetProfile_Should_ParseAllArguments() {
        // Act
        var execution = Parse(
            "--email", Email,
            "--hide-email",
            "--hide-linkedin",
            "--verbose"
        );

        // Assert
        var settings = GetSettings(execution);

        settings.Email.Should().Be(Email);
        settings.HideEmail.Should().BeTrue();
        settings.HideLinkedIn.Should().BeTrue();
        settings.Verbose.Should().BeTrue();
    }

    [Fact]
    public void GetProfile_Should_ParseShortOptionsTogether() {
        // Act
        var execution = Parse(
            "-e", Email,
            "-m",
            "-n",
            "-v"
        );

        // Assert
        var settings = GetSettings(execution);

        settings.Email.Should().Be(Email);
        settings.HideEmail.Should().BeTrue();
        settings.HideLinkedIn.Should().BeTrue();
        settings.Verbose.Should().BeTrue();
    }

    [Theory]
    [InlineData("--email")]
    [InlineData("-e")]
    public void GetProfile_Should_ParseEmail(string option) {
        // Act
        var execution = Parse(option, Email);

        // Assert
        var settings = GetSettings(execution);

        settings.Email.Should().Be(Email);
    }

    [Theory]
    [InlineData("--hide-email")]
    [InlineData("-m")]
    public void GetProfile_Should_ParseHideEmail(string option) {
        // Act
        var execution = Parse("--email", Email, option);

        // Assert
        var settings = GetSettings(execution);

        settings.HideEmail.Should().BeTrue();
    }

    [Theory]
    [InlineData("--hide-linkedin")]
    [InlineData("-n")]
    public void GetProfile_Should_ParseHideLinkedIn(string option) {
        // Act
        var execution = Parse("--email", Email, option);

        // Assert
        var settings = GetSettings(execution);

        settings.HideLinkedIn.Should().BeTrue();
    }

    [Theory]
    [InlineData("--verbose")]
    [InlineData("-v")]
    public void GetProfile_Should_ParseVerbose(string option) {
        // Act
        var execution = Parse("--email", Email, option);

        // Assert
        var settings = GetSettings(execution);

        settings.Verbose.Should().BeTrue();
    }

    [Fact]
    public void GetProfile_Should_UseDefaultValuesForOptionalArguments() {
        // Act
        var execution = Parse("--email", Email);

        // Assert
        var settings = GetSettings(execution);

        settings.HideEmail.Should().BeFalse();
        settings.HideLinkedIn.Should().BeFalse();
        settings.Verbose.Should().BeFalse();
    }
}
