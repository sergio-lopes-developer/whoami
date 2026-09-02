using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateFullName;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public sealed class UpdateFullNameWiringTests {
    private static UpdateFullNameCommand RunUpdateFullNameCommand(
        params string[] args
    ) {
        string[] commandArgs = ["profile", "update", "name", ..args];

        return CliApplicationCommandRunner
            .Run<UpdateFullNameCommand>(commandArgs);
    }

    [Fact]
    public void UpdateFullName_Should_ParseAllArguments() {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const string firstName = "John";
        const string lastName = "Doe";

        // Act
        var command = RunUpdateFullNameCommand(
            "--id", id,
            "--first-name", firstName,
            "--last-name", lastName
        );

        // Assert
        command.Id.Should().Be(id);
        command.FirstName.Should().Be(firstName);
        command.LastName.Should().Be(lastName);
    }

    [Theory]
    [InlineData("--id")]
    [InlineData("-i")]
    public void UpdateFullName_Should_ParseId(string option) {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        var command = RunUpdateFullNameCommand(option, id);

        // Assert
        command.Id.Should().Be(id);
    }

    [Theory]
    [InlineData("--first-name")]
    [InlineData("-f")]
    public void UpdateFullName_Should_ParseFirstName(string option) {
        // Arrange
        const string firstName = "John";

        // Act
        var command = RunUpdateFullNameCommand(option, firstName);

        // Assert
        command.FirstName.Should().Be(firstName);
    }

    [Theory]
    [InlineData("--last-name")]
    [InlineData("-l")]
    public void UpdateFullName_Should_ParseLastName(string option) {
        // Arrange
        const string lastName = "Doe";

        // Act
        var command = RunUpdateFullNameCommand(option, lastName);

        // Assert
        command.LastName.Should().Be(lastName);
    }
}
