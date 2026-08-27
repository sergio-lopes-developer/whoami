using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands.Profiles;

public class CreateProfileUnitTests {
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
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenCommandSucceeds() {
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
        var result = await _command.ExecuteInternalAsync(
            CommandContextFactory.Create("create"),
            settings,
            CancellationToken.None
        );

        // Assert
        result.Should().Be(0);

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
    public async Task ExecuteAsync_ShouldReturnError_WhenCommandFails() {
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
        var result = await _command.ExecuteInternalAsync(
            CommandContextFactory.Create("create"),
            settings,
            CancellationToken.None
        );

        result.Should().Be(1);

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
