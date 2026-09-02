using FluentAssertions;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Testing;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Settings.Profiles;

namespace WhoAmI.CLI.Tests.Parsing.Commands.Profiles;

public sealed class UpdateFullNameParsingTests {
    private const string Id = "11111111-1111-1111-1111-111111111111";
    private const string FirstName = "John";
    private const string LastName = "Doe";

    private static readonly CommandAppTester _tester = CreateTester();

    private static CommandAppTester CreateTester() {
        var tester = new CommandAppTester();

        tester.Configure(c => {
            c.AddBranch("profile", profile => {
                profile.AddBranch("update", update => {
                    update.AddCommand<UpdateFullName>("name");
                });
            });
        });

        return tester;
    }

    private static CommandAppResult Parse(params string[] args) =>
        _tester.Run(["profile", "update", "name", .. args]);

    private static UpdateFullNameCommandSettings GetSettings(
        CommandAppResult execution
    ) =>
        execution.Settings
            .Should()
            .BeOfType<UpdateFullNameCommandSettings>()
            .Subject;

    [Fact]
    public void UpdateFullName_Should_ParseAllArguments() {
        // Act
        var execution = Parse(
            "--id", Id,
            "--first-name", FirstName,
            "--last-name", LastName
        );

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
        settings.FirstName.Should().Be(FirstName);
        settings.LastName.Should().Be(LastName);
    }

    [Fact]
    public void UpdateFullName_Should_ParseShortOptionsTogether() {
        // Act
        var execution = Parse(
            "-i", Id,
            "-f", FirstName,
            "-l", LastName
        );

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
        settings.FirstName.Should().Be(FirstName);
        settings.LastName.Should().Be(LastName);
    }

    [Theory]
    [InlineData("--id")]
    [InlineData("-i")]
    public void UpdateFullName_Should_ParseId(string option) {
        // Act
        var execution = Parse(option, Id);

        // Assert
        var settings = GetSettings(execution);

        settings.Id.Should().Be(Id);
    }

    [Theory]
    [InlineData("--first-name")]
    [InlineData("-f")]
    public void UpdateFullName_Should_ParseFirstName(string option) {
        // Act
        var execution = Parse(option, FirstName);

        // Assert
        var settings = GetSettings(execution);

        settings.FirstName.Should().Be(FirstName);
    }

    [Theory]
    [InlineData("--last-name")]
    [InlineData("-l")]
    public void UpdateFullName_Should_ParseLastName(string option) {
        // Act
        var execution = Parse(option, LastName);

        // Assert
        var settings = GetSettings(execution);

        settings.LastName.Should().Be(LastName);
    }
}
