using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateFullName;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public sealed class UpdateFullNameWiringTests {
    private static UpdateFullNameCommand RunUpdateFullName(
        params string[] args
    ) {
        string[] commandArgs = ["profile", "update", "name", ..args];

        return CliApplicationCommandRunner
            .Run<UpdateFullNameCommand>(commandArgs);
    }

    [Fact]
    public void UpdateFullName_Should_BindCliArgumentsAndDispatchCommand() {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const string firstName = "John";
        const string lastName = "Doe";

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
