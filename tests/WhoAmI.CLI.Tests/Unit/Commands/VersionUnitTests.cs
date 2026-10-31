using System.Runtime.InteropServices;
using FluentAssertions;
using WhoAmI.CLI.Commands;
using WhoAmI.CLI.Hosting;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Settings;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands;

[Collection("CLI Console")]
public class VersionUnitTests {
    private const string Separator = "─";

    private readonly VersionCommand _command = new();

    private Task<(int ExitCode, string ConsoleOutput)> Execute(
        VersionCommandSettings settings
    ) =>
        CliCommandExecutor.Execute(
            settings,
            (context, s, token) =>
                Task.FromResult(_command.Execute(context, s, token))
        );

    [Fact]
    public async Task Execute_ShouldDisplayBasicVersionInformation_WhenVerboseOptionIsNotProvided() {
        // Arrange
        var settings = new VersionCommandSettings();

        // Act
        var (exitCode, output) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());

        output.Should().Contain("WhoAmI CLI v");
        output.Should().NotContain(Separator);

        output.Should().NotContain("Commit:");
        output.Should().NotContain(CliVersion.Commit);

        output.Should().NotContain("Environment:");
        output.Should().NotContain(RuntimeInformation.OSDescription);

        output.Should().NotContain("Architecture:");
        output.Should().NotContain(RuntimeInformation.OSArchitecture.ToString());

        output.Should().NotContain(".NET Runtime:");
        output.Should().NotContain(Environment.Version.ToString());
    }

    [Fact]
    public async Task Execute_ShouldDisplayVerboseInformation_WhenOptionIsProvided() {
        // Arrange
        var settings = new VersionCommandSettings {
            Verbose = true
        };

        // Act
        var (exitCode, output) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());

        output.Should().Contain("WhoAmI CLI v");
        output.Should().Contain(Separator);

        output.Should().Contain("Commit:");
        output.Should().Contain(CliVersion.Commit);

        output.Should().Contain("Environment:");
        output.Should().Contain(RuntimeInformation.OSDescription);

        output.Should().Contain("Architecture:");
        output.Should().Contain(RuntimeInformation.OSArchitecture.ToString());

        output.Should().Contain(".NET Runtime:");
        output.Should().Contain(Environment.Version.ToString());
    }
}
