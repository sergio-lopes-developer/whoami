using FluentAssertions;
using WhoAmI.Application.Features.Profiles.DeleteProfile;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public class DeleteProfileWiringTests {
    private static DeleteProfileCommand RunDeleteProfile(params string[] args) {
        string[] commandArgs = ["profile", "delete", ..args];

        return CliApplicationCommandRunner
            .Run<DeleteProfileCommand>(commandArgs);
    }

    [Fact]
    public void DeleteProfile_ShouldBindCliArgumentsAndDispatchCommand_WhenArgumentsAreProvided() {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        var command = RunDeleteProfile("--id", id);

        // Assert
        command.Id.Should().Be(id);
    }
}
