using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateEmail;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public sealed class UpdateEmailWiringTests {
    private static UpdateEmailCommand RunUpdateEmailCommand(
        params string[] args
    ) {
        string[] commandArgs = ["profile", "update", "email", ..args];

        return CliCommandRunner.Run<UpdateEmailCommand>(commandArgs);
    }

    [Fact]
    public void UpdateEmail_Should_ParseAllArguments() {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        var command = RunUpdateEmailCommand(
            "--id", id,
            "--email", "john@doe.com"
        );

        // Assert
        command.Id.Should().Be(id);
        command.Email.Should().Be("john@doe.com");
    }

    [Theory]
    [InlineData("--id")]
    [InlineData("-i")]
    public void UpdateEmail_Should_ParseId(string option) {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        var command = RunUpdateEmailCommand(option, id);

        // Assert
        command.Id.Should().Be(id);
    }

    [Theory]
    [InlineData("--email")]
    [InlineData("-e")]
    public void UpdateEmail_Should_ParseEmail(string option) {
        // Act
        var command = RunUpdateEmailCommand(option, "john@doe.com");

        // Assert
        command.Email.Should().Be("john@doe.com");
    }
}
