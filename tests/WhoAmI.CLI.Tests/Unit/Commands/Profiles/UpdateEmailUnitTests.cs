using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Features.Profiles.UpdateEmail;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands.Profiles;

public sealed class UpdateEmailUnitTests {
    private readonly ICommandDispatcher _dispatcher =
        DispatcherFactory.CreateCommandDispatcher();

    private readonly UpdateEmail _command;

    public UpdateEmailUnitTests() =>
        _command = new UpdateEmail(
            _dispatcher,
            LoggerFactory.Create<UpdateEmail>()
        );

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnSuccess_WhenCommandSucceeds() {
        // Arrange
        var settings = CommandSettingsFactory.UpdateEmail();

        _dispatcher
            .Send(Arg.Any<UpdateEmailCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());

        // Act
        var exit = await _command.ExecuteInternalAsync(
            CommandContextFactory.Create("update-email"),
            settings,
            CancellationToken.None
        );

        // Assert
        exit.Should().Be(CliExit.Success());

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
        var exit = await _command.ExecuteInternalAsync(
            CommandContextFactory.Create("update-email"),
            settings,
            CancellationToken.None
        );

        // Assert
        exit.Should().Be(CliExit.Error());

        await _dispatcher.Received(1).Send(
            Arg.Is<UpdateEmailCommand>(x =>
                x.Id == settings.Id &&
                x.Email == settings.Email
            ),
            Arg.Any<CancellationToken>()
        );
    }
}
