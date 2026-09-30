using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.DeleteProfile;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Settings.Profiles;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands.Profiles;

[Collection("CLI Console")]
public sealed class DeleteProfileUnitTests {
    private readonly ICommandDispatcher _dispatcher =
        DispatcherFactory.CreateCommandDispatcher();

    private readonly DeleteProfile _command;

    public DeleteProfileUnitTests() =>
        _command = new DeleteProfile(
            _dispatcher,
            LoggerFactory.Create<DeleteProfile>()
        );

    private Task<(int ExitCode, string ConsoleOutput)> Execute(
        DeleteProfileCommandSettings settings
    ) =>
        CliCommandExecutor.Execute(settings, _command.ExecuteInternalAsync);

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnSuccess_WhenCommandSucceeds() {
        // Arrange
        var settings = CommandSettingsFactory.DeleteProfile();

        _dispatcher
            .Send(Arg.Any<DeleteProfileCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());
        consoleOutput.Should().ContainAll("Profile successfully deleted.");

        await _dispatcher.Received(1).Send(
            Arg.Is<DeleteProfileCommand>(x => x.Id == settings.Id),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnError_WhenCommandFails() {
        // Arrange
        var settings = CommandSettingsFactory.DeleteProfile();

        _dispatcher
            .Send(Arg.Any<DeleteProfileCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure(
                ProfileErrors.AlreadyDeleted(settings.Id)
            ));

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Error());

        consoleOutput
            .Should()
            .ContainAll("The profile has already been deleted.");

        await _dispatcher.Received(1).Send(
            Arg.Is<DeleteProfileCommand>(x => x.Id == settings.Id),
            Arg.Any<CancellationToken>()
        );
    }
}
