using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateFullName;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public sealed class UpdateFullNameWiringTests {
    private static UpdateFullNameCommand RunUpdateFullNameCommand(
        params string[] args
    ) {
        string[] commandArgs = ["profile", "update", "name", ..args];

        return CliCommandRunner.Run<UpdateFullNameCommand>(commandArgs);
    }

    [Fact]
    public void UpdateFullName_Should_ParseAllArguments() {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        var command = RunUpdateFullNameCommand(
            "--id", id,
            "--first-name", "John",
            "--last-name", "Doe"
        );

        // Assert
        command.Id.Should().Be(id);
        command.FirstName.Should().Be("John");
        command.LastName.Should().Be("Doe");
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
        // Act
        var command = RunUpdateFullNameCommand(option, "John");

        // Assert
        command.FirstName.Should().Be("John");
    }

    [Theory]
    [InlineData("--last-name")]
    [InlineData("-l")]
    public void UpdateFullName_Should_ParseLastName(string option) {
        // Act
        var command = RunUpdateFullNameCommand(option, "Doe");

        // Assert
        command.LastName.Should().Be("Doe");
    }
}
