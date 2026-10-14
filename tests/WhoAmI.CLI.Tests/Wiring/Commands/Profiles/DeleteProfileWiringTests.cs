using FluentAssertions;
using WhoAmI.Application.Features.Profiles.DeleteProfile;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public class DeleteProfileWiringTests {
    private static DeleteProfileCommand RunDeleteProfile(
        params string[] commandOptions
    ) {
        string[] commandLineArguments = ["profile", "delete", ..commandOptions];

        return CliApplicationCommandRunner
            .Run<DeleteProfileCommand>(commandLineArguments);
    }

    [Fact]
    public void DeleteProfile_ShouldBindOptionsAndDispatchCommand_WhenOptionsAreProvided() {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        var command = RunDeleteProfile("--id", id);

        // Assert
        command.Id.Should().Be(id);
    }
}
