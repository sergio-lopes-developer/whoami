using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateEmail;

namespace WhoAmI.Application.Tests.Features.Profiles.UpdateEmail;

public class UpdateEmailCommandValidatorTests {
    private readonly UpdateEmailCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenCommandIsValid() {
        // Arrange
        var command = new UpdateEmailCommand(
            Guid.NewGuid(),
            "john.doe@email.com"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenIdIsEmpty() {
        // Arrange
        var command = new UpdateEmailCommand(
            Guid.Empty,
            "john.doe@email.com"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(command.Id));
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenEmailIsEmpty() {
        // Arrange
        var command = new UpdateEmailCommand(
            Guid.NewGuid(),
            string.Empty
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(command.Email));
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenEmailIsWhitespace() {
        // Arrange
        var command = new UpdateEmailCommand(
            Guid.NewGuid(),
            "   "
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(command.Email));
    }

    [Fact]
    public void Validate_ShouldReturnInvalidFormatError_WhenEmailIsInvalid() {
        // Arrange
        var command = new UpdateEmailCommand(
            Guid.NewGuid(),
            "invalid-email"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.InvalidFormat");
        error.Metadata!["Field"].Should().Be(nameof(command.Email));
    }

    [Fact]
    public void Validate_ShouldReturnTwoErrors_WhenIdAndEmailAreInvalid() {
        // Arrange
        var command = new UpdateEmailCommand(
            Guid.Empty,
            string.Empty
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().HaveCount(2);

        errors.Should().Contain(e =>
            e.Code == "Validation.Required" &&
            Equals(e.Metadata!["Field"], nameof(command.Id))
        );

        errors.Should().Contain(e =>
            e.Code == "Validation.Required" &&
            Equals(e.Metadata!["Field"], nameof(command.Email))
        );
    }

    [Fact]
    public void Validate_ShouldNotReturnInvalidFormat_WhenEmailIsEmpty() {
        // Arrange
        var command = new UpdateEmailCommand(
            Guid.NewGuid(),
            string.Empty
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }

    [Fact]
    public void Validate_ShouldNotReturnInvalidFormat_WhenEmailIsWhitespace() {
        // Arrange
        var command = new UpdateEmailCommand(
            Guid.NewGuid(),
            "   "
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }
}
