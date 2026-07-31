using FluentAssertions;
using WhoAmI.Application.Features.Profiles.UpdateSocialLinks;

namespace WhoAmI.Application.Tests.Features.Profiles.UpdateSocialLinks;

public class UpdateSocialLinksCommandValidatorTests {
    private readonly UpdateSocialLinksCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenCommandIsValid() {
        // Arrange
        var command = new UpdateSocialLinksCommand(
            Guid.NewGuid(),
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenIdIsEmpty() {
        // Arrange
        var command = new UpdateSocialLinksCommand(
            Guid.Empty,
            "https://www.linkedin.com/in/johnmiller",
            "https://github.com/johnmiller"
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
    public void Validate_ShouldReturnRequiredError_WhenLinkedInIsEmpty() {
        // Arrange
        var command = new UpdateSocialLinksCommand(
            Guid.NewGuid(),
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
        var command = new UpdateSocialLinksCommand(
            Guid.NewGuid(),
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
    public void Validate_ShouldReturnRequiredError_WhenLinkedInIsWhitespace() {
        // Arrange
        var command = new UpdateSocialLinksCommand(
            Guid.NewGuid(),
            "   ",
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
    public void Validate_ShouldReturnRequiredError_WhenGitHubIsWhitespace() {
        // Arrange
        var command = new UpdateSocialLinksCommand(
            Guid.NewGuid(),
            "https://www.linkedin.com/in/johnmiller",
            "   "
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
    public void Validate_ShouldReturnInvalidFormatError_WhenLinkedInIsInvalid() {
        // Arrange
        var command = new UpdateSocialLinksCommand(
            Guid.NewGuid(),
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
        var command = new UpdateSocialLinksCommand(
            Guid.NewGuid(),
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
    public void Validate_ShouldReturnThreeErrors_WhenAllFieldsAreInvalid() {
        // Arrange
        var command = new UpdateSocialLinksCommand(
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
            Equals(e.Metadata!["Field"], nameof(command.LinkedIn))
        );

        errors.Should().Contain(e =>
            e.Code == "Validation.Required" &&
            Equals(e.Metadata!["Field"], nameof(command.GitHub))
        );
    }

    [Fact]
    public void Validate_ShouldReturnTwoErrors_WhenBothUrlsAreInvalid() {
        // Arrange
        var command = new UpdateSocialLinksCommand(
            Guid.NewGuid(),
            "invalid-linkedin",
            "invalid-github"
        );

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().HaveCount(2);

        errors.Should().Contain(e =>
            e.Code == "Validation.InvalidFormat" &&
            Equals(e.Metadata!["Field"], nameof(command.LinkedIn))
        );

        errors.Should().Contain(e =>
            e.Code == "Validation.InvalidFormat" &&
            Equals(e.Metadata!["Field"], nameof(command.GitHub))
        );
    }

    [Fact]
    public void Validate_ShouldNotReturnDuplicateErrors_ForEmptyLinkedIn() {
        // Arrange
        var command = new UpdateSocialLinksCommand(
            Guid.NewGuid(),
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
        var command = new UpdateSocialLinksCommand(
            Guid.NewGuid(),
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
