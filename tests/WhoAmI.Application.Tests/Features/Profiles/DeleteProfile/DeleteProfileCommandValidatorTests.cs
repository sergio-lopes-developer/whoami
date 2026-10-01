using FluentAssertions;
using WhoAmI.Application.Features.Profiles.DeleteProfile;

namespace WhoAmI.Application.Tests.Features.Profiles.DeleteProfile;

public class DeleteProfileCommandValidatorTests {
    private readonly DeleteProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenCommandIsValid() {
        // Arrange
        var command = new DeleteProfileCommand(Guid.NewGuid());

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnRequiredError_WhenIdIsEmpty() {
        // Arrange
        var command = new DeleteProfileCommand(Guid.Empty);

        // Act
        var errors = _validator.Validate(command);

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
        error.Metadata!["Field"].Should().Be(nameof(command.Id));
    }
}
