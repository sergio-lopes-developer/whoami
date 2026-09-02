using FluentAssertions;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Testing;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Tests.Parsing.Commands.Profiles;

public sealed class UpdateEmailParsingTests {
    private const string Id = "11111111-1111-1111-1111-111111111111";
    private const string Email = "john@doe.com";

    private static readonly CommandAppTester _tester = CreateTester();

    private static CommandAppTester CreateTester() {
        var tester = new CommandAppTester();

        tester.Configure(c => {
            c.AddBranch("profile", profile => {
                profile.AddBranch("update", update => {
                    update.AddCommand<UpdateEmail>("email");
                });
            });
        });

        return tester;
    }

    private static CommandAppResult Parse(params string[] args) =>
        _tester.Run(["profile", "update", "email", .. args]);

    private static UpdateEmailCommandSettings GetSettings(
        CommandAppResult execution
    ) =>
        execution.Settings
            .Should()
            .BeOfType<UpdateEmailCommandSettings>()
            .Subject;

    [Fact]
    public void UpdateEmail_Should_ParseAllArguments() {
        // Act
        var execution = Parse(
            "--id", Id,
            "--email", Email
        );

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
        settings.Email.Should().Be(Email);
    }

    [Fact]
    public void UpdateEmail_Should_ParseShortOptionsTogether() {
        // Act
        var execution = Parse(
            "-i", Id,
            "-e", Email
        );

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
        settings.Email.Should().Be(Email);
    }

    [Theory]
    [InlineData("--id")]
    [InlineData("-i")]
    public void UpdateEmail_Should_ParseId(string option) {
        // Act
        var execution = Parse(option, Id);

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
    }

    [Theory]
    [InlineData("--email")]
    [InlineData("-e")]
    public void UpdateEmail_Should_ParseEmail(string option) {
        // Act
        var execution = Parse(option, Email);

        // Assert
        var settings = GetSettings(execution);

        settings.Email.Should().Be(Email);
    }
}
