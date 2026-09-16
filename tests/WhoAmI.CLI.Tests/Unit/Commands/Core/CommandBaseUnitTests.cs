using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Spectre.Console.Cli;
using WhoAmI.CLI.Commands.Core;
using WhoAmI.CLI.Output;
using WhoAmI.Domain.Shared.Exceptions;

namespace WhoAmI.CLI.Tests.Unit.Commands.Core;

[Collection("CLI Console")]
public sealed class CommandBaseUnitTests {
    public sealed class TestCommand(
        ILogger<TestCommand> logger,
        Func<CommandContext, TestSettings, CancellationToken, Task<int>> execute
    ) : CommandBase<TestCommand, TestSettings>(logger) {
        public Task<int> ExecuteForTestAsync(
            CommandContext context,
            TestSettings settings,
            CancellationToken cancellationToken
        ) =>
            ExecuteAsync(context, settings, cancellationToken);

        protected override Task<int> ExecuteCommandAsync(
            CommandContext context,
            TestSettings settings,
            CancellationToken cancellationToken
        ) =>
            execute(context, settings, cancellationToken);
    }

    public sealed class TestSettings : CommandSettings;

    public sealed class TestDomainException(string message)
        : DomainException(message);

    private static CommandContext CreateContext() =>
        new(
            [],
            Substitute.For<IRemainingArguments>(),
            "test",
            null
        );

    [Fact]
    public async Task ExecuteAsync_ShouldReturnCommandResult() {
        // Arrange
        var logger = Substitute.For<ILogger<TestCommand>>();

        var command = new TestCommand(
            logger,
            (_, _, _) => Task.FromResult(123)
        );

        // Act
        var result = await command.ExecuteForTestAsync(
            CreateContext(),
            new TestSettings(),
            CancellationToken.None
        );

        // Assert
        result.Should().Be(123);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenOperationIsCancelled() {
        // Arrange
        var logger = Substitute.For<ILogger<TestCommand>>();

        var command = new TestCommand(
            logger,
            (_, _, _) => throw new OperationCanceledException()
        );

        // Act
        var result = await command.ExecuteForTestAsync(
            CreateContext(),
            new TestSettings(),
            CancellationToken.None
        );

        // Assert
        result.Should().Be(CliExit.Success());

        logger.DidNotReceiveWithAnyArgs()
            .Log(
                default,
                default,
                null,
                null,
                null!
            );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenDomainExceptionIsThrown() {
        // Arrange
        var logger = Substitute.For<ILogger<TestCommand>>();

        var command = new TestCommand(
            logger,
            (_, _, _) => throw new TestDomainException("Something went wrong.")
        );

        // Act
        var result = await command.ExecuteForTestAsync(
            CreateContext(),
            new TestSettings(),
            CancellationToken.None
        );

        // Assert
        result.Should().Be(CliExit.Success());

        logger.DidNotReceiveWithAnyArgs()
            .Log(
                default,
                default,
                null,
                null,
                null!
            );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnError_WhenUnhandledExceptionIsThrown() {
        // Arrange
        var logger = Substitute.For<ILogger<TestCommand>>();

        var exception = new InvalidOperationException("Boom.");

        var command = new TestCommand(
            logger,
            (_, _, _) => throw exception
        );

        // Act
        var result = await command.ExecuteForTestAsync(
            CreateContext(),
            new TestSettings(),
            CancellationToken.None
        );

        // Assert
        result.Should().Be(CliExit.Error());
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogUnhandledException() {
        // Arrange
        var logger = Substitute.For<ILogger<TestCommand>>();

        var exception = new InvalidOperationException("Boom.");

        var command = new TestCommand(
            logger,
            (_, _, _) => throw exception
        );

        // Act
        await command.ExecuteForTestAsync(
            CreateContext(),
            new TestSettings(),
            CancellationToken.None
        );

        // Assert
        logger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            exception,
            Arg.Any<Func<object, Exception?, string>>()
        );
    }
}
