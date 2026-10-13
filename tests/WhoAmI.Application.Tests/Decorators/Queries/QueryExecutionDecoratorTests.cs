using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Execution;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Decorators.Queries;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Tests.Decorators.Queries;

public class QueryExecutionDecoratorTests {
    public sealed record FakeQuery : IQuery<FakeResult>;

    public sealed record FakeResult;

    [Fact]
    public async Task HandleAsync_ShouldDelegateExecutionToRunner() {
        // Arrange
        var query = new FakeQuery();
        var expected = Result<FakeResult>.Success(new FakeResult());
        var inner = Substitute.For<IQueryHandler<FakeQuery, FakeResult>>();
        var runner = Substitute.For<IExecutionRunner>();

        runner
            .ExecuteAsync(
                nameof(FakeQuery),
                query,
                Arg.Any<Func<Task<Result<FakeResult>>>>()
            )
            .Returns(expected);

        var decorator = new QueryExecutionDecorator<FakeQuery, FakeResult>(
            inner,
            runner
        );

        // Act
        var result = await decorator.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.Should().BeSameAs(expected);

        await runner.Received(1)
            .ExecuteAsync(
                nameof(FakeQuery),
                query,
                Arg.Any<Func<Task<Result<FakeResult>>>>()
            );
    }

    [Fact]
    public async Task HandleAsync_ShouldInvokeInnerHandler_WhenRunnerExecutesDelegate() {
        // Arrange
        var query = new FakeQuery();
        var expected = Result<FakeResult>.Success(new FakeResult());
        var inner = Substitute.For<IQueryHandler<FakeQuery, FakeResult>>();

        inner
            .HandleAsync(query, Arg.Any<CancellationToken>())
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

        var decorator = new QueryExecutionDecorator<FakeQuery, FakeResult>(
            inner,
            runner
        );

        // Act
        var result = await decorator.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.Should().BeSameAs(expected);

        await inner.Received(1)
            .HandleAsync(query, Arg.Any<CancellationToken>());
    }
}
