using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Decorators.Commands;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Tests.Decorators.Commands;

public class CommandValidationDecoratorTests {
    public sealed record FakeCommand(string Value) : ICommand;

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenValidationFails() {
        // Arrange
        var command = new FakeCommand("invalid");

        var error = new Error(
            "Validation.Required",
            "Value is required."
        );

        var validator = Substitute.For<ICommandValidator<FakeCommand>>();

        validator.Validate(command).Returns([error]);

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        var decorator = new CommandValidationDecorator<FakeCommand>(
            inner,
            [validator]
        );

        // Act
        var result = await decorator.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Should().Be(error);

        await inner.DidNotReceive()
            .HandleAsync(
                Arg.Any<FakeCommand>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task HandleAsync_ShouldInvokeInnerHandler_WhenValidationSucceeds() {
        // Arrange
        var command = new FakeCommand("valid");

        var validator = Substitute.For<ICommandValidator<FakeCommand>>();

        validator.Validate(command).Returns([]);

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        inner.HandleAsync(command, Arg.Any<CancellationToken>())
            .Returns(Result.Success());

        var decorator = new CommandValidationDecorator<FakeCommand>(
            inner,
            [validator]
        );

        // Act
        var result = await decorator.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsSuccess.Should().BeTrue();

        await inner.Received(1)
            .HandleAsync(command, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldAggregateErrors_FromAllValidators() {
        // Arrange
        var command = new FakeCommand("invalid");

        var error1 = new Error(
            "Validation.Required",
            "Value is required."
        );

        var error2 = new Error(
            "Validation.InvalidFormat",
            "Value format is invalid."
        );

        var validator1 = Substitute.For<ICommandValidator<FakeCommand>>();

        validator1.Validate(command).Returns([error1]);

        var validator2 = Substitute.For<ICommandValidator<FakeCommand>>();

        validator2.Validate(command).Returns([error2]);

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        var decorator = new CommandValidationDecorator<FakeCommand>(
            inner,
            [validator1, validator2]
        );

        // Act
        var result = await decorator.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Errors.Should().HaveCount(2);

        result.Errors.Should().Contain(error1);
        result.Errors.Should().Contain(error2);

        await inner.DidNotReceive()
            .HandleAsync(
                Arg.Any<FakeCommand>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task HandleAsync_ShouldRemoveEquivalentErrors() {
        // Arrange
        var command = new FakeCommand("invalid");

        var error1 = new Error(
            "Validation.Required",
            "Value is required."
        );

        var error2 = new Error(
            "Validation.Required",
            "Value is required."
        );

        var validator1 = Substitute.For<ICommandValidator<FakeCommand>>();

        validator1.Validate(command).Returns([error1]);

        var validator2 = Substitute.For<ICommandValidator<FakeCommand>>();

        validator2.Validate(command).Returns([error2]);

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        var decorator = new CommandValidationDecorator<FakeCommand>(
            inner,
            [validator1, validator2]
        );

        // Act
        var result = await decorator.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Errors.Should().ContainSingle();

        await inner.DidNotReceive()
            .HandleAsync(
                Arg.Any<FakeCommand>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task HandleAsync_ShouldPropagateException_WhenInnerThrows() {
        // Arrange
        var command = new FakeCommand("valid");

        var validator = Substitute.For<ICommandValidator<FakeCommand>>();

        validator.Validate(command).Returns([]);

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        inner.HandleAsync(command, Arg.Any<CancellationToken>())
            .Returns(
                Task.FromException<Result>(new InvalidOperationException())
            );

        var decorator = new CommandValidationDecorator<FakeCommand>(
            inner,
            [validator]
        );

        // Act
        var act = () => decorator.HandleAsync(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
