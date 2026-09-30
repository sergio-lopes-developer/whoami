using FluentAssertions;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Testing;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Tests.Parsing.Commands.Profiles;

public sealed class DeleteProfileParsingTests {
    private const string Id = "11111111-1111-1111-1111-111111111111";

    private static readonly CommandAppTester _tester = CreateTester();

    private static CommandAppTester CreateTester() {
        var tester = new CommandAppTester();

        tester.Configure(c => {
            c.AddBranch("profile", profile => {
                profile.AddCommand<DeleteProfile>("delete");
            });
        });

        return tester;
    }

    private static CommandAppResult Parse(params string[] args) =>
        _tester.Run(["profile", "delete", .. args]);

    private static DeleteProfileCommandSettings GetSettings(
        CommandAppResult execution
    ) =>
        execution.Settings
            .Should()
            .BeOfType<DeleteProfileCommandSettings>()
            .Subject;

    [Fact]
    public void DeleteProfile_Should_ParseAllArguments() {
        // Act
        var execution = Parse("--id", Id);

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
    }

    [Fact]
    public void DeleteProfile_Should_ParseShortOptionsTogether() {
        // Act
        var execution = Parse("-i", Id);

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
    }

    [Theory]
    [InlineData("--id")]
    [InlineData("-i")]
    public void DeleteProfile_Should_ParseId(string option) {
        // Act
        var execution = Parse(option, Id);

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
    }
}
