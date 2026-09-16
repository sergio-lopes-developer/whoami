using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Features.Profiles.UpdateFullName;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Settings.Profiles;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands.Profiles;

[Collection("CLI Console")]
public sealed class UpdateFullNameUnitTests {
    private readonly ICommandDispatcher _dispatcher =
        DispatcherFactory.CreateCommandDispatcher();

    private readonly UpdateFullName _command;

    public UpdateFullNameUnitTests() =>
        _command = new UpdateFullName(
            _dispatcher,
            LoggerFactory.Create<UpdateFullName>()
        );

    private Task<(int ExitCode, string ConsoleOutput)> Execute(
        UpdateFullNameCommandSettings settings
    ) =>
        CliCommandExecutor.Execute(settings, _command.ExecuteInternalAsync);

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnSuccess_WhenCommandSucceeds() {
        // Arrange
        var settings = CommandSettingsFactory.UpdateFullName();

        _dispatcher
            .Send(
                Arg.Any<UpdateFullNameCommand>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(Result.Success());

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());
        consoleOutput.Should().ContainAll("Name successfully updated.");

        await _dispatcher.Received(1).Send(
            Arg.Is<UpdateFullNameCommand>(x =>
                x.Id == settings.Id &&
                x.FirstName == settings.FirstName &&
                x.LastName == settings.LastName
            ),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnError_WhenCommandFails() {
        // Arrange
        var settings = CommandSettingsFactory.UpdateFullName();

        _dispatcher
            .Send(
                Arg.Any<UpdateFullNameCommand>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(Result.Failure(
                new Error("Profile.NotFound", "Profile not found."))
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Error());
        consoleOutput.Should().ContainAll("Profile not found.");

        await _dispatcher.Received(1).Send(
            Arg.Is<UpdateFullNameCommand>(x =>
                x.Id == settings.Id &&
                x.FirstName == settings.FirstName &&
                x.LastName == settings.LastName
            ),
            Arg.Any<CancellationToken>()
        );
    }
}
