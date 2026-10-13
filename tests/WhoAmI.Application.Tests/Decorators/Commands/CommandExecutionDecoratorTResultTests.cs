using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Execution;
using WhoAmI.Application.Decorators.Commands;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Tests.Decorators.Commands;

public class CommandExecutionDecoratorTResultTests {
    public sealed record FakeCommand : ICommand<FakeResult>;

    public sealed record FakeResult;

    [Fact]
    public async Task HandleAsync_ShouldDelegateExecutionToRunner() {
        // Arrange
        var command = new FakeCommand();
        var expected = Result<FakeResult>.Success(new FakeResult());
        var inner = Substitute.For<ICommandHandler<FakeCommand, FakeResult>>();
        var runner = Substitute.For<IExecutionRunner>();

        runner
            .ExecuteAsync(
                nameof(FakeCommand),
                command,
                Arg.Any<Func<Task<Result<FakeResult>>>>()
            )
            .Returns(expected);

        var decorator = new CommandExecutionDecorator<FakeCommand, FakeResult>(
            inner,
            runner
        );

        // Act
        var result = await decorator.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.Should().BeSameAs(expected);

        await runner.Received(1)
            .ExecuteAsync(
                nameof(FakeCommand),
                command,
                Arg.Any<Func<Task<Result<FakeResult>>>>()
            );
    }

    [Fact]
    public async Task HandleAsync_ShouldInvokeInnerHandler_WhenRunnerExecutesDelegate() {
        // Arrange
        var command = new FakeCommand();
        var expected = Result<FakeResult>.Success(new FakeResult());
        var inner = Substitute.For<ICommandHandler<FakeCommand, FakeResult>>();

        inner
            .HandleAsync(command, Arg.Any<CancellationToken>())
            .Returns(expected);

        var runner = Substitute.For<IExecutionRunner>();

        runner
            .ExecuteAsync(
                Arg.Any<string>(),
                Arg.Any<object>(),
                Arg.Any<Func<Task<Result<FakeResult>>>>()
            )
            .Returns(
                callInfo => callInfo.Arg<Func<Task<Result<FakeResult>>>>()()
            );

        var decorator = new CommandExecutionDecorator<FakeCommand, FakeResult>(
            inner,
            runner
        );

        // Act
        var result = await decorator.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.Should().BeSameAs(expected);

        await inner.Received(1)
            .HandleAsync(command, Arg.Any<CancellationToken>());
    }
}
