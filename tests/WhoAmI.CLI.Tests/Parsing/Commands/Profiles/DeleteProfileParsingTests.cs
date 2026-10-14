using FluentAssertions;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Testing;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Tests.Parsing.Commands.Profiles;

public class DeleteProfileParsingTests {
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

    private static CommandAppResult Parse(params string[] commandOptions) =>
        _tester.Run(["profile", "delete", .. commandOptions]);

    private static DeleteProfileCommandSettings GetSettings(
        CommandAppResult execution
    ) =>
        execution.Settings
            .Should()
            .BeOfType<DeleteProfileCommandSettings>()
            .Subject;

    [Theory]
    [InlineData("--id")]
    [InlineData("-i")]
    public void DeleteProfile_ShouldParseId_WhenOptionIsProvided(
        string option
    ) {
        // Act
        var execution = Parse(option, Id);

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
    }
}
