using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Persistence;
using WhoAmI.Application.Decorators.Commands;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Tests.Decorators.Commands;

public class CommandUnitOfWorkDecoratorTests {
    public sealed record FakeCommand : ICommand;

    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenInnerSucceeds() {
        // Arrange
        var command = new FakeCommand();

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        inner.HandleAsync(command, Arg.Any<CancellationToken>())
            .Returns(Result.Success());

        var uow = Substitute.For<IUnitOfWork>();

        uow.CommitAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

        var decorator = new CommandUnitOfWorkDecorator<FakeCommand>(
            inner,
            uow
        );

        // Act
        var result = await decorator.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsSuccess.Should().BeTrue();

        await inner
            .Received(1).HandleAsync(command, Arg.Any<CancellationToken>());

        await uow.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldCommit_AfterInnerHandler() {
        // Arrange
        var command = new FakeCommand();

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        var uow = Substitute.For<IUnitOfWork>();

        var callOrder = new List<string>();

        inner.HandleAsync(command, Arg.Any<CancellationToken>())
            .Returns(_ => {
                callOrder.Add("handler");
                return Result.Success();
            });

        uow.CommitAsync(Arg.Any<CancellationToken>()).Returns(_ => {
            callOrder.Add("uow");
            return Task.FromResult(Result.Success());
        });

        var decorator = new CommandUnitOfWorkDecorator<FakeCommand>(
            inner,
            uow
        );

        // Act
        await decorator.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        callOrder.Should().ContainInOrder("handler", "uow");
    }

    [Fact]
    public async Task HandleAsync_ShouldNotCallSaveChanges_WhenInnerFails() {
        // Arrange
        var command = new FakeCommand();

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        inner.HandleAsync(command, Arg.Any<CancellationToken>()).Returns(
            Result.Failure(new Error("Fake.Error", "Fake failure."))
        );

        var uow = Substitute.For<IUnitOfWork>();

        var decorator = new CommandUnitOfWorkDecorator<FakeCommand>(
            inner,
            uow
        );

        // Act
        var result = await decorator.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();

        await uow
            .DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldPropagateException_WhenInnerThrows() {
        // Arrange
        var command = new FakeCommand();

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        inner.HandleAsync(command, Arg.Any<CancellationToken>()).Returns(
            Task.FromException<Result>(new InvalidOperationException())
        );

        var uow = Substitute.For<IUnitOfWork>();

        var decorator = new CommandUnitOfWorkDecorator<FakeCommand>(
            inner,
            uow
        );

        // Act
        var act = () => decorator.HandleAsync(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();

        await uow
            .DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenSaveChangesFails() {
        // Arrange
        var command = new FakeCommand();

        var inner = Substitute.For<ICommandHandler<FakeCommand>>();

        inner.HandleAsync(command, Arg.Any<CancellationToken>())
            .Returns(Result.Success());

        var expectedError = new Error("Persistence.Error", "Database failure.");

        var uow = Substitute.For<IUnitOfWork>();

        uow.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(Result.Failure(expectedError));

        var decorator = new CommandUnitOfWorkDecorator<FakeCommand>(
            inner,
            uow
        );

        // Act
        var result = await decorator.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError.Should().Be(expectedError);
    }
}
