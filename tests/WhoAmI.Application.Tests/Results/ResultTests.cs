using FluentAssertions;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Tests.Results;

public class ResultTests {
    [Fact]
    public void Success_ShouldCreateSuccessfulResult() {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
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
        var result = Result.Failure(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();

        result.Errors.Should().ContainSingle();
        result.FirstError.Should().Be(error);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult_WhenMultipleErrorsProvided() {
        // Arrange
        var errors = new List<Error> {
            new("Error.One", "First error."),
            new("Error.Two", "Second error.")
        };

        // Act
        var result = Result.Failure(errors);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();

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
        Result result = error;

        // Assert
        result.IsFailure.Should().BeTrue();

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
        var result = Result.Failure([first, second]);

        // Assert
        result.FirstError.Should().Be(first);
    }

    [Fact]
    public void Failure_ShouldThrow_WhenErrorCollectionIsEmpty() {
        // Arrange
        var errors = Array.Empty<Error>();

        // Act
        var act = () => Result.Failure(errors);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Failure result must contain errors.");
    }
}
