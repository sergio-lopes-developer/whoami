using FluentAssertions;
using Spectre.Console.Cli.Testing;
using WhoAmI.CLI.Commands;
using WhoAmI.CLI.Settings;

namespace WhoAmI.CLI.Tests.Parsing.Commands;

public class VersionParsingTests {
    private static readonly CommandAppTester _tester = CreateTester();

    private static CommandAppTester CreateTester() {
        var tester = new CommandAppTester();

        tester.Configure(c => {
            c.AddCommand<VersionCommand>("version");
        });

        return tester;
    }

    private static CommandAppResult Parse(params string[] commandOptions) =>
        _tester.Run(["version", ..commandOptions]);

    private static VersionCommandSettings GetSettings(
        CommandAppResult execution
    ) =>
        execution.Settings
            .Should()
            .BeOfType<VersionCommandSettings>()
            .Subject;

    [Theory]
    [InlineData("--verbose")]
    [InlineData("-v")]
    public void Version_ShouldParseVerbose_WhenOptionIsProvided(
        string option
    ) {
        // Act
        var execution = Parse(option);

        // Assert
        var settings = GetSettings(execution);

        settings.Verbose.Should().BeTrue();
    }

    [Fact]
    public void Version_ShouldUseDefaultOptionValues_WhenOptionalOptionsAreNotProvided() {
        // Act
        var execution = Parse();

        // Assert
        var settings = GetSettings(execution);

        settings.Verbose.Should().BeFalse();
    }
}
