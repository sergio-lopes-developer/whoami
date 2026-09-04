using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands.Profiles;

public sealed class CreateProfileUnitTests {
    private readonly ICommandDispatcher _dispatcher =
        DispatcherFactory.CreateCommandDispatcher();

    private readonly CreateProfile _command;

    public CreateProfileUnitTests() {
        _command = new CreateProfile(
            _dispatcher,
            LoggerFactory.Create<CreateProfile>()
        );
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnSuccess_WhenCommandSucceeds() {
        // Arrange
        var settings = CommandSettingsFactory.CreateProfile();

        var response = new CreateProfileResponse(
            Guid.NewGuid(),
            settings.FirstName,
            settings.LastName,
            settings.Email,
            settings.LinkedIn,
            settings.GitHub
        );

        _dispatcher
            .Send(Arg.Any<CreateProfileCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<CreateProfileResponse>.Success(response));

        // Act
        var exit = await _command.ExecuteInternalAsync(
            CommandContextFactory.Create("create"),
            settings,
            CancellationToken.None
        );

        // Assert
        exit.Should().Be(CliExit.Success());

        await _dispatcher.Received(1).Send(
            Arg.Is<CreateProfileCommand>(x =>
                x.FirstName == settings.FirstName &&
                x.LastName == settings.LastName &&
                x.Email == settings.Email &&
                x.LinkedIn == settings.LinkedIn &&
                x.GitHub == settings.GitHub
            ),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnError_WhenCommandFails() {
        // Arrange
        var settings = CommandSettingsFactory.CreateProfile();

        _dispatcher
            .Send(Arg.Any<CreateProfileCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<CreateProfileResponse>.Failure(
                new Error(
                    "Profile.DuplicateEmail",
                    "The email must be unique.",
                    new Dictionary<string, object?> {
                        ["ProfileEmail"] = settings.Email
                    }
                )
            ));

        // Act
        var exit = await _command.ExecuteInternalAsync(
            CommandContextFactory.Create("create"),
            settings,
            CancellationToken.None
        );

        exit.Should().Be(CliExit.Error());

        // Assert
        await _dispatcher.Received(1).Send(
            Arg.Is<CreateProfileCommand>(x =>
                x.FirstName == settings.FirstName &&
                x.LastName == settings.LastName &&
                x.Email == settings.Email &&
                x.LinkedIn == settings.LinkedIn &&
                x.GitHub == settings.GitHub
            ),
            Arg.Any<CancellationToken>()
        );
    }
}
