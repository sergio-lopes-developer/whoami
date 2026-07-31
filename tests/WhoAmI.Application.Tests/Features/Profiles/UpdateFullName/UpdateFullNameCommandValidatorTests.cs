using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateFullName;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Tests.Features.Profiles.UpdateFullName;

public class UpdateFullNameCommandValidatorTests {
    private readonly UpdateFullNameCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenCommandIsValid() {
        // Arrange
        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            "John",
            "Miller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenIdIsEmpty() {
        // Arrange
        var command = new UpdateFullNameCommand(
            Guid.Empty,
            "John",
            "Miller"
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
    public void Validate_ShouldReturnRequiredError_WhenFirstNameIsEmpty() {
        // Arrange
        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            string.Empty,
            "Miller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(command.FirstName));
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenLastNameIsEmpty() {
        // Arrange
        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            "John",
            string.Empty
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(command.LastName));
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenFirstNameIsWhitespace() {
        // Arrange
        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            "   ",
            "Miller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(command.FirstName));
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenLastNameIsWhitespace() {
        // Arrange
        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            "John",
            "   "
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(command.LastName));
    }

    [Fact]
    public void Validate_ShouldReturnInvalidLengthError_WhenFirstNameIsTooShort() {
        // Arrange
        var firstName = new string(
            'A',
            FirstName.LengthConstraint.MinLength - 1
        );

        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            firstName,
            "Miller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.InvalidLength");
        error.Metadata!["Field"].Should().Be(nameof(command.FirstName));
    }

    [Fact]
    public void Validate_ShouldReturnInvalidLengthError_WhenFirstNameIsTooLong() {
        // Arrange
        var firstName = new string(
            'A',
            FirstName.LengthConstraint.MaxLength + 1
        );

        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            firstName,
            "Miller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.InvalidLength");
        error.Metadata!["Field"].Should().Be(nameof(command.FirstName));
    }

    [Fact]
    public void Validate_ShouldReturnInvalidLengthError_WhenLastNameIsTooShort() {
        // Arrange
        var lastName = new string(
            'A',
            LastName.LengthConstraint.MinLength - 1
        );

        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            "John",
            lastName
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.InvalidLength");
        error.Metadata!["Field"].Should().Be(nameof(command.LastName));
    }

    [Fact]
    public void Validate_ShouldReturnInvalidLengthError_WhenLastNameIsTooLong() {
        // Arrange
        var lastName = new string(
            'A',
            LastName.LengthConstraint.MaxLength + 1
        );

        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            "John",
            lastName
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.InvalidLength");
        error.Metadata!["Field"].Should().Be(nameof(command.LastName));
    }

    [Fact]
    public void Validate_ShouldReturnTwoErrors_WhenBothNamesAreInvalid() {
        // Arrange
        var firstName = new string(
            'A',
            FirstName.LengthConstraint.MinLength - 1
        );

        var lastName = new string(
            'A',
            LastName.LengthConstraint.MinLength - 1
        );

        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            firstName,
            lastName
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().HaveCount(2);

        errors.Should().Contain(e =>
            e.Code == "Validation.InvalidLength" &&
            Equals(
                e.Metadata!["Field"],
                nameof(command.FirstName)
            )
        );

        errors.Should().Contain(e =>
            e.Code == "Validation.InvalidLength" &&
            Equals(
                e.Metadata!["Field"],
                nameof(command.LastName)
            )
        );
    }

    [Fact]
    public void Validate_ShouldReturnThreeErrors_WhenAllFieldsAreInvalid() {
        // Arrange
        var command = new UpdateFullNameCommand(
            Guid.Empty,
            string.Empty,
            string.Empty
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().HaveCount(3);

        errors.Should().Contain(e =>
            e.Code == "Validation.Required" &&
            Equals(e.Metadata!["Field"], nameof(command.Id))
        );

        errors.Should().Contain(e =>
            e.Code == "Validation.Required" &&
            Equals(e.Metadata!["Field"], nameof(command.FirstName))
        );

        errors.Should().Contain(e =>
            e.Code == "Validation.Required" &&
            Equals(e.Metadata!["Field"], nameof(command.LastName))
        );
    }

    [Fact]
    public void Validate_ShouldNotReturnInvalidLength_WhenFirstNameIsEmpty() {
        // Arrange
        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            string.Empty,
            "Miller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }

    [Fact]
    public void Validate_ShouldNotReturnInvalidLength_WhenLastNameIsEmpty() {
        // Arrange
        var command = new UpdateFullNameCommand(
            Guid.NewGuid(),
            "John",
            string.Empty
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }
}
