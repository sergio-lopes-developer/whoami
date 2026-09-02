using FluentAssertions;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands;

public sealed class VersionCommandWiringTests {
    [Theory]
    [InlineData("version")]
    [InlineData("v")]
    public void Version_Should_BeRegistered(string command) {
        // Act
        var exit = CliRunner.Run(command);

        // Assert
        exit.Should().Be(CliExit.Success());
    }
}
