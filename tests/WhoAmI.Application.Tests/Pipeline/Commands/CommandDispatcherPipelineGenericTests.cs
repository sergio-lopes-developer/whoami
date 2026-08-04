using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Abstractions.Persistence;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.Application.Results;
using WhoAmI.Application.Tests.TestInfrastructure;
using WhoAmI.Domain.Profiles;

namespace WhoAmI.Application.Tests.Pipeline.Commands;

public class CommandDispatcherPipelineGenericTests {
    [Fact]
    public async Task Pipeline_ShouldCallCommit_WhenCommandSucceeds() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        var uow = Substitute.For<IUnitOfWork>();

        uow.CommitAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new CreateProfileCommand(
            "John",
            "Doe",
            "john@doe.com",
            "https://linkedin.com/in/john",
            "https://github.com/john"
        );

        // Act
        var result = await dispatcher.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await uow.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Pipeline_ShouldReturnValidationFailure_WhenCommandIsInvalid() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        var uow = Substitute.For<IUnitOfWork>();

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new CreateProfileCommand(
            "",
            "",
            "invalid-email",
            "",
            ""
        );

        // Act
        var result = await dispatcher.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();

        await uow.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Pipeline_ShouldReturnFailure_WhenPersistenceFails() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        var error = new Error(
            "Persistence.Error",
            "Database failure."
        );

        var uow = Substitute.For<IUnitOfWork>();

        uow.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(Result.Failure(error));

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new CreateProfileCommand(
            "John",
            "Doe",
            "john@doe.com",
            "https://linkedin.com/in/john",
            "https://github.com/john"
        );

        // Act
        var result = await dispatcher.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError.Should().Be(error);
    }

    [Fact]
    public async Task Pipeline_ShouldRethrowUnexpectedExceptions_FromPersistence() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        var exception = new InvalidOperationException();

        var uow = Substitute.For<IUnitOfWork>();

        uow.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Result>(exception));

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new CreateProfileCommand(
            "John",
            "Doe",
            "john@doe.com",
            "https://linkedin.com/in/john",
            "https://github.com/john"
        );

        // Act
        var act = () => dispatcher.Send(command);

        // Assert
        var thrown = await act.Should().ThrowAsync<InvalidOperationException>();

        thrown.Which.Should().BeSameAs(exception);
    }

    [Fact]
    public async Task Pipeline_ShouldInvokeHandlerExactlyOnce() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        var uow = Substitute.For<IUnitOfWork>();

        uow.CommitAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new CreateProfileCommand(
            "John",
            "Doe",
            "john@doe.com",
            "https://linkedin.com/in/john",
            "https://github.com/john"
        );

        // Act
        await dispatcher.Send(command);

        // Assert
        await uow.Received(1).CommitAsync(Arg.Any<CancellationToken>());

        repo.Received(1).Add(Arg.Any<Profile>());
    }

    [Fact]
    public async Task Pipeline_ShouldCallCommit_AfterHandler() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        var executionOrder = new List<string>();

        repo.When(x => x.Add(Arg.Any<Profile>()))
            .Do(_ => executionOrder.Add("handler"));

        var uow = Substitute.For<IUnitOfWork>();

        uow.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(_ => {
                executionOrder.Add("uow");
                return Result.Success();
            });

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new CreateProfileCommand(
            "John",
            "Doe",
            "john@doe.com",
            "https://linkedin.com/in/john",
            "https://github.com/john"
        );

        // Act
        await dispatcher.Send(command);

        // Assert
        executionOrder.Should().ContainInOrder("handler", "uow");
    }
}
