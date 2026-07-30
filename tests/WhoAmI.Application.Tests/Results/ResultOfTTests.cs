using FluentAssertions;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Tests.Results;

public class ResultOfTTests {
    [Fact]
    public void Success_ShouldCreateSuccessfulResult_WithValue() {
        // Arrange
        const string value = "Success";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();

        result.Value.Should().Be(value);

        result.Errors.Should().BeEmpty();
        result.FirstError.Should().BeNull();
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult_WhenSingleErrorProvided() {
        // Arrange
        var error = new Error(
            "Test.Error",
            "Something went wrong."
        );

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();

        result.Value.Should().BeNull();

        result.Errors.Should().ContainSingle();
        result.FirstError.Should().Be(error);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult_WhenMultipleErrorsProvided() {
        // Arrange
        var errors = new List<Error> {
            new(
                "Error.One",
                "First error."
            ),
            new(
                "Error.Two",
                "Second error."
            )
        };

        // Act
        var result = Result<string>.Failure(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();

        result.Value.Should().BeNull();

        result.Errors.Should().HaveCount(2);

        result.Errors.Should().Contain(errors[0]);
        result.Errors.Should().Contain(errors[1]);

        result.FirstError.Should().Be(errors[0]);
    }

    [Fact]
    public void ImplicitOperator_ShouldCreateFailureResult_FromError() {
        // Arrange
        var error = new Error(
            "Test.Error",
            "Something went wrong."
        );

        // Act
        Result<string> result = error;

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Value.Should().BeNull();

        result.Errors.Should().ContainSingle();
        result.FirstError.Should().Be(error);
    }

    [Fact]
    public void FirstError_ShouldReturnFirstError_WhenMultipleErrorsExist() {
        // Arrange
        var first = new Error(
            "Error.One",
            "First error."
        );

        var second = new Error(
            "Error.Two",
            "Second error."
        );

        // Act
        var result = Result<string>.Failure(
            [first, second]
        );

        // Assert
        result.FirstError.Should().Be(first);
    }

    [Fact]
    public void Success_ShouldStoreProvidedValue() {
        // Arrange
        var value = Guid.NewGuid();

        // Act
        var result = Result<Guid>.Success(value);

        // Assert
        result.Value.Should().Be(value);
    }

    [Fact]
    public void Failure_ShouldThrow_WhenErrorCollectionIsEmpty() {
        // Arrange
        var errors = Array.Empty<Error>();

        // Act
        var act = () => Result<string>.Failure(errors);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Failure result must contain errors.");
    }

    [Fact]
    public void Success_ShouldThrow_WhenValueIsNull() {
        // Act
        var act = () => Result<string>.Success(null!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Success result must contain a value.");
    }
}
