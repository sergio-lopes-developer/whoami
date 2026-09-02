using FluentAssertions;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Testing;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Tests.Parsing.Commands.Profiles;

public sealed class UpdateSocialLinksParsingTests {
    private const string Id = "11111111-1111-1111-1111-111111111111";
    private const string LinkedIn = "https://www.linkedin.com/in/username";
    private const string GitHub = "https://github.com/username";

    private static readonly CommandAppTester _tester = CreateTester();

    private static CommandAppTester CreateTester() {
        var tester = new CommandAppTester();

        tester.Configure(c => {
            c.AddBranch("profile", profile => {
                profile.AddBranch("update", update => {
                    update.AddCommand<UpdateSocialLinks>("social-links");
                });
            });
        });

        return tester;
    }

    private static CommandAppResult Parse(params string[] args) =>
        _tester.Run(["profile", "update", "social-links", .. args]);

    private static UpdateSocialLinksCommandSettings GetSettings(
        CommandAppResult execution
    ) =>
        execution.Settings
            .Should()
            .BeOfType<UpdateSocialLinksCommandSettings>()
            .Subject;

    [Fact]
    public void UpdateSocialLinks_Should_ParseAllArguments() {
        // Act
        var execution = Parse(
            "--id", Id,
            "--linkedin", LinkedIn,
            "--github", GitHub
        );

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
        settings.LinkedIn.Should().Be(LinkedIn);
        settings.GitHub.Should().Be(GitHub);
    }

    [Fact]
    public void UpdateSocialLinks_Should_ParseShortOptionsTogether() {
        // Act
        var execution = Parse(
            "-i", Id,
            "-n", LinkedIn,
            "-g", GitHub
        );

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
        settings.LinkedIn.Should().Be(LinkedIn);
        settings.GitHub.Should().Be(GitHub);
    }

    [Theory]
    [InlineData("--id")]
    [InlineData("-i")]
    public void UpdateSocialLinks_Should_ParseId(string option) {
        // Act
        var execution = Parse(option, Id);

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
    }

    [Theory]
    [InlineData("--linkedin")]
    [InlineData("-n")]
    public void UpdateSocialLinks_Should_ParseLinkedIn(string option) {
        // Act
        var execution = Parse(option, LinkedIn);

        // Assert
        var settings = GetSettings(execution);

        settings.LinkedIn.Should().Be(LinkedIn);
    }

    [Theory]
    [InlineData("--github")]
    [InlineData("-g")]
    public void UpdateSocialLinks_Should_ParseGitHub(string option) {
        // Act
        var execution = Parse(option, GitHub);

        // Assert
        var settings = GetSettings(execution);

        settings.GitHub.Should().Be(GitHub);
    }
}
