using FluentAssertions;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Tests.Features.Profiles.CreateProfile;

public class CreateProfileCommandValidatorTests {
    private readonly CreateProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenCommandIsValid() {
        // Arrange
        var command = new CreateProfileCommand(
            "John",
            "Miller",
            "john.miller@email.com",
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenFirstNameIsEmpty() {
        // Arrange
        var command = new CreateProfileCommand(
            string.Empty,
            "Miller",
            "john.miller@email.com",
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
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
        var command = new CreateProfileCommand(
            "John",
            string.Empty,
            "john.miller@email.com",
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
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
    public void Validate_ShouldReturnRequiredError_WhenEmailIsEmpty() {
        // Arrange
        var command = new CreateProfileCommand(
            "John",
            "Miller",
            string.Empty,
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
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
    public void Validate_ShouldReturnRequiredError_WhenLinkedInIsEmpty() {
        // Arrange
        var command = new CreateProfileCommand(
            "John",
            "Miller",
            "john.miller@email.com",
            string.Empty,
            "https://github.com/johnmiller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(command.LinkedIn));
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenGitHubIsEmpty() {
        // Arrange
        var command = new CreateProfileCommand(
            "John",
            "Miller",
            "john.miller@email.com",
            "https://www.linkedin.com/in/johnmiller",
            string.Empty
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(command.GitHub));
    }

    [Fact]
    public void Validate_ShouldReturnInvalidLengthError_WhenFirstNameIsTooShort() {
        // Arrange
        var firstName = new string(
            'A',
            FirstName.LengthConstraint.MinLength - 1
        );

        var command = new CreateProfileCommand(
            firstName,
            "Miller",
            "john.miller@email.com",
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
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

        var command = new CreateProfileCommand(
            firstName,
            "Miller",
            "john.miller@email.com",
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
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

        var command = new CreateProfileCommand(
            "John",
            lastName,
            "john.miller@email.com",
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
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

        var command = new CreateProfileCommand(
            "John",
            lastName,
            "john.miller@email.com",
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
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
    public void Validate_ShouldReturnInvalidFormatError_WhenEmailIsInvalid() {
        // Arrange
        var command = new CreateProfileCommand(
            "John",
            "Miller",
            "invalid-email",
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
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
    public void Validate_ShouldReturnInvalidFormatError_WhenLinkedInIsInvalid() {
        // Arrange
        var command = new CreateProfileCommand(
            "John",
            "Miller",
            "john.miller@email.com",
            "not-a-url",
            "https://github.com/johnmiller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.InvalidFormat");
        error.Metadata!["Field"].Should().Be(nameof(command.LinkedIn));
    }

    [Fact]
    public void Validate_ShouldReturnInvalidFormatError_WhenGitHubIsInvalid() {
        // Arrange
        var command = new CreateProfileCommand(
            "John",
            "Miller",
            "john.miller@email.com",
            "https://www.linkedin.com/in/johnmiller",
            "not-a-url"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.InvalidFormat");
        error.Metadata!["Field"].Should().Be(nameof(command.GitHub));
    }

    [Fact]
    public void Validate_ShouldReturnFiveErrors_WhenAllFieldsAreInvalid() {
        // Arrange
        var command = new CreateProfileCommand(
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().HaveCount(5);

        errors.Should().Contain(e =>
            e.Code == "Validation.Required" &&
            Equals(e.Metadata!["Field"], nameof(command.FirstName)));

        errors.Should().Contain(e =>
            e.Code == "Validation.Required" &&
            Equals(e.Metadata!["Field"], nameof(command.LastName)));

        errors.Should().Contain(e =>
            e.Code == "Validation.Required" &&
            Equals(e.Metadata!["Field"], nameof(command.Email)));

        errors.Should().Contain(e =>
            e.Code == "Validation.Required" &&
            Equals(e.Metadata!["Field"], nameof(command.LinkedIn)));

        errors.Should().Contain(e =>
            e.Code == "Validation.Required" &&
            Equals(e.Metadata!["Field"], nameof(command.GitHub)));
    }

    [Fact]
    public void Validate_ShouldNotReturnDuplicateErrors_ForEmptyEmail() {
        // Arrange
        var command = new CreateProfileCommand(
            "John",
            "Miller",
            string.Empty,
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }

    [Fact]
    public void Validate_ShouldNotReturnDuplicateErrors_ForEmptyLinkedIn() {
        // Arrange
        var command = new CreateProfileCommand(
            "John",
            "Miller",
            "john.miller@email.com",
            string.Empty,
            "https://github.com/johnmiller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }

    [Fact]
    public void Validate_ShouldNotReturnDuplicateErrors_ForEmptyGitHub() {
        // Arrange
        var command = new CreateProfileCommand(
            "John",
            "Miller",
            "john.miller@email.com",
            "https://www.linkedin.com/in/johnmiller",
            string.Empty
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }
}
