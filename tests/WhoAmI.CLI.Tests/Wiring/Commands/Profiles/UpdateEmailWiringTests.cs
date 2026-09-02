using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateEmail;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public sealed class UpdateEmailWiringTests {
    private static UpdateEmailCommand RunUpdateEmail(params string[] args) {
        string[] commandArgs = ["profile", "update", "email", ..args];

        return CliApplicationCommandRunner.Run<UpdateEmailCommand>(commandArgs);
    }

    [Fact]
    public void UpdateEmail_Should_BindCliArgumentsAndDispatchCommand() {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const string email = "john@doe.com";

        // Act
        var command = RunUpdateEmail(
            "--id", id,
            "--email", email
        );

        // Assert
        command.Id.Should().Be(id);
        command.Email.Should().Be(email);
    }
}
