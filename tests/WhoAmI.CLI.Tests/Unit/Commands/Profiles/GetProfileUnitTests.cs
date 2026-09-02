using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands.Profiles;

public sealed class GetProfileUnitTests {
    private readonly IQueryDispatcher _dispatcher =
        DispatcherFactory.CreateQueryDispatcher();

    private readonly GetProfile _command;

    public GetProfileUnitTests() {
        _command = new GetProfile(
            _dispatcher,
            LoggerFactory.Create<GetProfile>()
        );
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnSuccess_WhenQuerySucceeds() {
        // Arrange
        var settings = CommandSettingsFactory.GetProfile();

        _dispatcher
            .Send(
                Arg.Any<GetProfileByEmailQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                Result<GetProfileByEmailResponse>.Success(
                    new GetProfileByEmailResponse(
                        Guid.Empty,
                        "John",
                        "Doe",
                        settings.Email,
                        "linkedin",
                        "Github"
                    )
                )
            );

        // Act
        var exit = await _command.ExecuteInternalAsync(
            CommandContextFactory.Create("get"),
            settings,
            CancellationToken.None
        );

        // Assert
        exit.Should().Be(CliExit.Success());

        await _dispatcher.Received(1).Send(
            Arg.Is<GetProfileByEmailQuery>(q => q.Email == settings.Email),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnError_WhenQueryFails() {
        // Arrange
        var settings = CommandSettingsFactory.GetProfile();

        _dispatcher
            .Send(
                Arg.Any<GetProfileByEmailQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                Result<GetProfileByEmailResponse>.Failure(
                    new Error(
                        "Profile.NotFound",
                        "Profile not found."
                    )
                )
            );

        // Act
        var exit = await _command.ExecuteInternalAsync(
            CommandContextFactory.Create("get"),
            settings,
            CancellationToken.None
        );

        // Assert
        exit.Should().Be(CliExit.Error());

        await _dispatcher.Received(1).Send(
            Arg.Is<GetProfileByEmailQuery>(q => q.Email == settings.Email),
            Arg.Any<CancellationToken>()
        );
    }
}
