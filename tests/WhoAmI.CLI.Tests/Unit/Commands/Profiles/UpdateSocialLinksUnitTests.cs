using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Features.Profiles.UpdateSocialLinks;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Settings.Profiles;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands.Profiles;

[Collection("CLI Console")]
public sealed class UpdateSocialLinksUnitTests {
    private readonly ICommandDispatcher _dispatcher =
        DispatcherFactory.CreateCommandDispatcher();

    private readonly UpdateSocialLinks _command;

    public UpdateSocialLinksUnitTests() =>
        _command = new UpdateSocialLinks(
            _dispatcher,
            LoggerFactory.Create<UpdateSocialLinks>()
        );

    private Task<(int ExitCode, string ConsoleOutput)> Execute(
        UpdateSocialLinksCommandSettings settings
    ) =>
        CliCommandExecutor.Execute(settings, _command.ExecuteInternalAsync);

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnSuccess_WhenCommandSucceeds() {
        // Arrange
        var settings = CommandSettingsFactory.UpdateSocialLinks();

        _dispatcher
            .Send(
                Arg.Any<UpdateSocialLinksCommand>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(Result.Success());

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());
        consoleOutput.Should().Contain("Social links successfully updated.");

        await _dispatcher.Received(1).Send(
            Arg.Is<UpdateSocialLinksCommand>(x =>
                x.Id == settings.Id &&
                x.LinkedIn == settings.LinkedIn &&
                x.GitHub == settings.GitHub
            ),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnError_WhenCommandFails() {
        // Arrange
        var settings = CommandSettingsFactory.UpdateSocialLinks();

        _dispatcher
            .Send(
                Arg.Any<UpdateSocialLinksCommand>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(Result.Failure(
                new Error("Profile.NotFound", "Profile was not found."))
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Error());
        consoleOutput.Should().Contain("Profile was not found.");

        await _dispatcher.Received(1).Send(
            Arg.Is<UpdateSocialLinksCommand>(x =>
                x.Id == settings.Id &&
                x.LinkedIn == settings.LinkedIn &&
                x.GitHub == settings.GitHub
            ),
            Arg.Any<CancellationToken>()
        );
    }
}
