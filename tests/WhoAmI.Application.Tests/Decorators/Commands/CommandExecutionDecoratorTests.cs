using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Decorators.Commands;
using WhoAmI.Application.Logging;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Tests.Decorators.Commands;

public class CommandExecutionDecoratorTests {
    public sealed record FakeCommand : ICommand;

    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenInnerHandlerSucceeds() {
        // Arrange
        var command = new FakeCommand();

        var info = new ExecutionInfo(
            Operation: nameof(FakeCommand),
            Request: command,
            Success: true,
            ElapsedMilliseconds: 10
        );

        var logger = Substitute.For<ISafeExecutionLogger>();

        var executionInfoFactory = Substitute.For<IExecutionInfoFactory>();

        executionInfoFactory.Create(
            Arg.Any<string>(),
            Arg.Any<object>(),
            Arg.Any<Result>(),
            Arg.Any<long>()
        ).Returns(info);

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        inner.HandleAsync(command, Arg.Any<CancellationToken>())
            .Returns(Result.Success());

        var decorator = new CommandExecutionDecorator<FakeCommand>(
            inner,
            logger,
            executionInfoFactory
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

        executionInfoFactory.Received(1).Create(
            nameof(FakeCommand),
            command,
            Arg.Is<Result>(r => r != null && r.IsSuccess),
            Arg.Is<long>(elapsed => elapsed >= 0)
        );

        logger.Received(1).Log(Arg.Any<ExecutionInfo>());
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenInnerHandlerFails() {
        // Arrange
        var command = new FakeCommand();

        var error = new Error(
            "Fake.Error",
            "Something failed."
        );

        var info = new ExecutionInfo(
            Operation: nameof(FakeCommand),
            Request: command,
            Success: false,
            ElapsedMilliseconds: 10,
            Errors: [error]
        );

        var logger = Substitute.For<ISafeExecutionLogger>();

        var executionInfoFactory = Substitute.For<IExecutionInfoFactory>();

        executionInfoFactory.Create(
            Arg.Any<string>(),
            Arg.Any<object>(),
            Arg.Any<Result>(),
            Arg.Any<long>()
        ).Returns(info);

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        inner.HandleAsync(command, Arg.Any<CancellationToken>())
            .Returns(Result.Failure(error));

        var decorator = new CommandExecutionDecorator<FakeCommand>(
            inner,
            logger,
            executionInfoFactory
        );

        // Act
        var result = await decorator.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError.Should().Be(error);

        await inner.Received(1)
            .HandleAsync(command, Arg.Any<CancellationToken>());

        executionInfoFactory.Received(1).Create(
            nameof(FakeCommand),
            command,
            Arg.Is<Result>(r =>
                r != null &&
                r.IsFailure &&
                r.FirstError == error
            ),
            Arg.Is<long>(elapsed => elapsed >= 0)
        );

        logger.Received(1).Log(Arg.Any<ExecutionInfo>());
    }

    [Fact]
    public async Task HandleAsync_ShouldPropagateException_WhenInnerHandlerThrows() {
        // Arrange
        var command = new FakeCommand();

        var exception = new InvalidOperationException();

        var info = new ExecutionInfo(
            Operation: nameof(FakeCommand),
            Request: command,
            Success: false,
            ElapsedMilliseconds: 10,
            Exception: exception
        );

        var logger = Substitute.For<ISafeExecutionLogger>();

        var executionInfoFactory = Substitute.For<IExecutionInfoFactory>();

        executionInfoFactory.Create(
            Arg.Any<string>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<long>()
        ).Returns(info);

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        inner.HandleAsync(command, Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Result>(exception));

        var decorator = new CommandExecutionDecorator<FakeCommand>(
            inner,
            logger,
            executionInfoFactory
        );

        // Act
        var act = () => decorator.HandleAsync(command);

        // Assert
        var thrown = await act.Should().ThrowAsync<InvalidOperationException>();

        thrown.Which.Should().BeSameAs(exception);

        executionInfoFactory.Received(1).Create(
            nameof(FakeCommand),
            command,
            exception,
            Arg.Is<long>(elapsed => elapsed >= 0)
        );

        logger.Received(1).Log(Arg.Any<ExecutionInfo>());
    }
}
