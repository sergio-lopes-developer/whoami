using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Features.Profiles.UpdateEmail;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Settings.Profiles;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands.Profiles;

[Collection("CLI Console")]
public sealed class UpdateEmailUnitTests {
    private readonly ICommandDispatcher _dispatcher =
        DispatcherFactory.CreateCommandDispatcher();

    private readonly UpdateEmail _command;

    public UpdateEmailUnitTests() =>
        _command = new UpdateEmail(
            _dispatcher,
            LoggerFactory.Create<UpdateEmail>()
        );

    private Task<(int ExitCode, string ConsoleOutput)> Execute(
        UpdateEmailCommandSettings settings
    ) =>
        CliCommandExecutor.Execute(settings, _command.ExecuteInternalAsync);

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnSuccess_WhenCommandSucceeds() {
        // Arrange
        var settings = CommandSettingsFactory.UpdateEmail();

        _dispatcher
            .Send(Arg.Any<UpdateEmailCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());
        consoleOutput.Should().ContainAll("Email successfully updated.");

        await _dispatcher.Received(1).Send(
            Arg.Is<UpdateEmailCommand>(x =>
                x.Id == settings.Id &&
                x.Email == settings.Email
            ),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnError_WhenCommandFails() {
        // Arrange
        var settings = CommandSettingsFactory.UpdateEmail();

        _dispatcher
            .Send(Arg.Any<UpdateEmailCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure(
                new Error("InvalidEmail", "Invalid email."))
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Error());
        consoleOutput.Should().ContainAll("Invalid email.");

        await _dispatcher.Received(1).Send(
            Arg.Is<UpdateEmailCommand>(x =>
                x.Id == settings.Id &&
                x.Email == settings.Email
            ),
            Arg.Any<CancellationToken>()
        );
    }
}
