using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateFullName;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public class UpdateFullNameWiringTests {
    private static UpdateFullNameCommand RunUpdateFullName(
        params string[] args
    ) {
        string[] commandArgs = ["profile", "update", "name", ..args];

        return CliApplicationCommandRunner
            .Run<UpdateFullNameCommand>(commandArgs);
    }

    [Fact]
    public void UpdateFullName_ShouldBindCliArgumentsAndDispatchCommand_WhenArgumentsAreProvided() {
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
