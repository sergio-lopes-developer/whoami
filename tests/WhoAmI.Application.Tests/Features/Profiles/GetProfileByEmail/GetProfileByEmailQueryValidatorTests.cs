using FluentAssertions;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;

namespace WhoAmI.Application.Tests.Features.Profiles.GetProfileByEmail;

public class GetProfileByEmailQueryValidatorTests {
    private readonly GetProfileByEmailQueryValidator _validator = new();

        [Fact]
    public void Validate_ShouldReturnNoErrors_WhenEmailIsValid() {
        // Arrange
        var query = new GetProfileByEmailQuery("john.doe@email.com");

        // Act
        var errors = _validator.Validate(query);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenEmailIsEmpty() {
        // Arrange
        var query = new GetProfileByEmailQuery(string.Empty);

        // Act
        var errors = _validator.Validate(query);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(query.Email));
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenEmailIsWhitespace() {
        // Arrange
        var query = new GetProfileByEmailQuery("   ");

        // Act
        var errors = _validator.Validate(query);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(query.Email));
    }

    [Fact]
    public void Validate_ShouldReturnInvalidFormatError_WhenEmailIsInvalid() {
        // Arrange
        var query = new GetProfileByEmailQuery("invalid-email");

        // Act
        var errors = _validator.Validate(query);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.InvalidFormat");
        error.Metadata!["Field"].Should().Be(nameof(query.Email));
    }

    [Fact]
    public void Validate_ShouldNotReturnInvalidFormat_WhenEmailIsEmpty() {
        // Arrange
        var query = new GetProfileByEmailQuery(string.Empty);

        // Act
        var errors = _validator.Validate(query);

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }

    [Fact]
    public void Validate_ShouldNotReturnInvalidFormat_WhenEmailIsWhitespace() {
        // Arrange
        var query = new GetProfileByEmailQuery("   ");

        // Act
        var errors = _validator.Validate(query);

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }
}
