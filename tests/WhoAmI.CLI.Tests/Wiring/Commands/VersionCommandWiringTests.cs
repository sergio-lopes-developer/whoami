using FluentAssertions;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands;

public sealed class VersionCommandWiringTests {
    [Fact]
    public void Version_Should_BeRegistered() {
        // Act
        var exit = CliCommandRunner.Run("version");

        // Assert
        exit.Should().Be(CliExit.Success());
    }

    [Fact]
    public void Version_Should_BeRegisteredWithAlias() {
        // Act
        var exit = CliCommandRunner.Run("v");

        // Assert
        exit.Should().Be(CliExit.Success());
    }
}
