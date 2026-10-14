using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateFullName;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public class UpdateFullNameWiringTests {
    private static UpdateFullNameCommand RunUpdateFullName(
        params string[] commandOptions
    ) {
        string[] commandLineArguments = [
            "profile",
            "update",
            "name",
            ..commandOptions
        ];

        return CliApplicationCommandRunner
            .Run<UpdateFullNameCommand>(commandLineArguments);
    }

    [Fact]
    public void UpdateFullName_ShouldBindOptionsAndDispatchCommand_WhenOptionsAreProvided() {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";
        var id = Guid.NewGuid().ToString();

        // Act
        var command = RunUpdateFullName(
            "--id", id,
            "--first-name", firstName,
            "--last-name", lastName
        );

        // Assert
        command.Id.Should().Be(id);
        command.FirstName.Should().Be(firstName);
        command.LastName.Should().Be(lastName);
    }
}
