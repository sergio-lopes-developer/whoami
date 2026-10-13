using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Execution;
using WhoAmI.Application.Logging;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Tests.Execution;

public class ExecutionRunnerTests {
    private sealed record FakeResult;

    private const string Operation = "FakeOperation";

    private readonly object _request = new();

    [Fact]
    public async Task ExecuteAsync_ShouldReturnSuccessResult_WhenHandlerSucceeds() {
        // Arrange
        var result = Result.Success();

        var info = new ExecutionInfo(
            Operation: Operation,
            Request: _request,
            Success: true,
            ElapsedMilliseconds: 10
        );

        var logger = Substitute.For<ISafeExecutionLogger>();
        var executionInfoFactory = Substitute.For<IExecutionInfoFactory>();

        executionInfoFactory
            .Create(
                Operation,
                _request,
                Arg.Any<Result>(),
                Arg.Any<long>()
            )
            .Returns(info);

        var runner = new ExecutionRunner(logger, executionInfoFactory);

        // Act
        var executionResult = await runner.ExecuteAsync(
            Operation,
            _request,
            () => Task.FromResult(result)
        );

        // Assert
        executionResult.Should().BeSameAs(result);

        executionInfoFactory.Received(1).Create(
            Operation,
            _request,
            result,
            Arg.Is<long>(e => e >= 0)
        );

        logger.Received(1).Log(info);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnTypedResult_WhenHandlerSucceeds() {
        // Arrange
        var value = new FakeResult();
        var result = Result<FakeResult>.Success(value);

        var info = new ExecutionInfo(
            Operation: Operation,
            Request: _request,
            Success: true,
            ElapsedMilliseconds: 10
        );

        var logger = Substitute.For<ISafeExecutionLogger>();
        var executionInfoFactory = Substitute.For<IExecutionInfoFactory>();

        executionInfoFactory
            .Create(
                Operation,
                _request,
                Arg.Any<Result<FakeResult>>(),
                Arg.Any<long>()
            )
            .Returns(info);

        var runner = new ExecutionRunner(logger, executionInfoFactory);

        // Act
        var executionResult = await runner.ExecuteAsync(
            Operation,
            _request,
            () => Task.FromResult(result)
        );

        // Assert
        executionResult.Should().BeSameAs(result);

        executionInfoFactory.Received(1).Create(
            Operation,
            _request,
            result,
            Arg.Is<long>(e => e >= 0)
        );

        logger.Received(1).Log(info);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogFailure_WhenHandlerReturnsFailure() {
        // Arrange
        var error = new Error("Fake.Error", "Something failed.");
        var result = Result.Failure(error);

        var info = new ExecutionInfo(
            Operation: Operation,
            Request: _request,
            Success: false,
            ElapsedMilliseconds: 10,
            Errors: [error]
        );

        var logger = Substitute.For<ISafeExecutionLogger>();
        var executionInfoFactory = Substitute.For<IExecutionInfoFactory>();

        executionInfoFactory
            .Create(
                Operation,
                _request,
                Arg.Any<Result>(),
                Arg.Any<long>()
            )
            .Returns(info);

        var runner = new ExecutionRunner(logger, executionInfoFactory);

        // Act
        var executionResult = await runner.ExecuteAsync(
            Operation,
            _request,
            () => Task.FromResult(result)
        );

        // Assert
        executionResult.Should().BeSameAs(result);

        executionInfoFactory.Received(1).Create(
            Operation,
            _request,
            result,
            Arg.Is<long>(e => e >= 0)
        );

        logger.Received(1).Log(info);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogExceptionAndRethrow_WhenHandlerThrows() {
        // Arrange
        var exception = new InvalidOperationException();

        var info = new ExecutionInfo(
            Operation: Operation,
            Request: _request,
            Success: false,
            ElapsedMilliseconds: 10,
            Exception: exception
        );

        var logger = Substitute.For<ISafeExecutionLogger>();
        var executionInfoFactory = Substitute.For<IExecutionInfoFactory>();

        executionInfoFactory
            .Create(
                Operation,
                _request,
                exception,
                Arg.Any<long>()
            )
            .Returns(info);

        var runner = new ExecutionRunner(logger, executionInfoFactory);

        // Act
        var act = () => runner.ExecuteAsync(
            Operation,
            _request,
            () => Task.FromException<Result>(exception)
        );

        // Assert
        var thrown = await act.Should().ThrowAsync<InvalidOperationException>();

        thrown.Which.Should().BeSameAs(exception);

        executionInfoFactory.Received(1).Create(
            Operation,
            _request,
            exception,
            Arg.Is<long>(e => e >= 0)
        );

        logger.Received(1).Log(info);
    }
}
