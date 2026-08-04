using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Abstractions.Persistence;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.UpdateEmail;
using WhoAmI.Application.Results;
using WhoAmI.Application.Tests.TestInfrastructure;
using WhoAmI.Domain.Profiles;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Application.Tests.Pipeline.Commands;

public class CommandDispatcherPipelineTests {
    [Fact]
    public async Task Pipeline_ShouldCallCommit_WhenCommandSucceeds() {
        // Arrange
        var profile = ProfileFactory.Create();

        var repo = Substitute.For<IProfileRepository>();

        repo.GetByIdAsync(profile.Id, Arg.Any<CancellationToken>())
            .Returns(profile);

        var uow = Substitute.For<IUnitOfWork>();

        uow.CommitAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new UpdateEmailCommand(profile.Id, "new@email.com");

        // Act
        var result = await dispatcher.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        await uow.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Pipeline_ShouldReturnFailure_WhenProfileDoesNotExist() {
        // Arrange
        var profile = ProfileFactory.Create();

        var repo = Substitute.For<IProfileRepository>();

        repo.GetByIdAsync(profile.Id, Arg.Any<CancellationToken>())
            .Returns((Profile?)null);

        var uow = Substitute.For<IUnitOfWork>();

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new UpdateEmailCommand(profile.Id, "duplicate@email.com");

        // Act
        var result = await dispatcher.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.NotFound");
        result.FirstError.Metadata!["ProfileId"].Should().Be(profile.Id);

        await uow.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Pipeline_ShouldReturnFailure_WhenPersistenceFails() {
        // Arrange
        var profile = ProfileFactory.Create();

        var duplicateEmail = "new@email.com";

        var repo = Substitute.For<IProfileRepository>();

        repo.GetByIdAsync(profile.Id, Arg.Any<CancellationToken>())
            .Returns(profile);

        var uow = Substitute.For<IUnitOfWork>();

        uow.CommitAsync(Arg.Any<CancellationToken>()).Returns(
            Result.Failure(ProfileErrors.DuplicateEmail(new(duplicateEmail)))
        );

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new UpdateEmailCommand(profile.Id, duplicateEmail);

        // Act
        var result = await dispatcher.Send(command);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.FirstError!.Code.Should().Be("Profile.DuplicateEmail");
        result.FirstError.Metadata!["ProfileEmail"].Should().Be(duplicateEmail);
    }

    [Fact]
    public async Task Pipeline_ShouldRethrowUnexpectedExceptions_FromPersistence() {
        // Arrange
        var profile = ProfileFactory.Create();

        var repo = Substitute.For<IProfileRepository>();

        repo.GetByIdAsync(profile.Id, Arg.Any<CancellationToken>())
            .Returns(profile);

        var uow = Substitute.For<IUnitOfWork>();
        var exception = new InvalidOperationException();

        uow.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Result>(exception));

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new UpdateEmailCommand(profile.Id, "new@email.com");

        // Act
        var act = () => dispatcher.Send(command);

        // Assert
        var thrown = await act.Should().ThrowAsync<InvalidOperationException>();
        thrown.Which.Should().BeSameAs(exception);
    }

    [Fact]
    public async Task Pipeline_ShouldInvokeHandlerExactlyOnce() {
        // Arrange
        var profile = ProfileFactory.Create();

        var repo = Substitute.For<IProfileRepository>();

        repo.GetByIdAsync(profile.Id, Arg.Any<CancellationToken>())
            .Returns(profile);

        var uow = Substitute.For<IUnitOfWork>();

        uow.CommitAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new UpdateEmailCommand(profile.Id, "new@email.com");

        // Act
        await dispatcher.Send(command);

        // Assert
        await uow.Received(1).CommitAsync(Arg.Any<CancellationToken>());

        await repo
            .Received(1).GetByIdAsync(profile.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Pipeline_ShouldCallCommit_AfterHandler() {
        // Arrange
        var profile = ProfileFactory.Create();

        var executionOrder = new List<string>();

        var repo = Substitute.For<IProfileRepository>();

        repo.GetByIdAsync(profile.Id, Arg.Any<CancellationToken>())
            .Returns(_ => {
                executionOrder.Add("handler");
                return profile;
            });

        var uow = Substitute.For<IUnitOfWork>();
        uow.CommitAsync(Arg.Any<CancellationToken>())
            .Returns(_ => {
                executionOrder.Add("uow");
                return Result.Success();
            });

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var dispatcher = provider.GetRequiredService<ICommandDispatcher>();

        var command = new UpdateEmailCommand(profile.Id, "new@email.com");

        // Act
        await dispatcher.Send(command);

        // Assert
        executionOrder.Should().ContainInOrder("handler", "uow");
    }
}
