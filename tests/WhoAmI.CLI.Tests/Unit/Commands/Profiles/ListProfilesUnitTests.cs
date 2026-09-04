using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Features.Profiles.ListProfiles;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands.Profiles;

public sealed class ListProfilesUnitTests {
    private readonly IQueryDispatcher _dispatcher =
        DispatcherFactory.CreateQueryDispatcher();

    private readonly ListProfiles _command;

    public ListProfilesUnitTests() {
        _command = new ListProfiles(
            _dispatcher,
            LoggerFactory.Create<ListProfiles>()
        );
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnSuccess_WhenQuerySucceeds() {
        // Arrange
        var settings = CommandSettingsFactory.ListProfiles();

        _dispatcher
            .Send(
                Arg.Any<ListProfilesQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                Result<IReadOnlyCollection<ListProfilesResponse>>.Success(
                    [
                        new(
                            "John Doe",
                            "john@doe.com"
                        ),
                        new(
                            "Jane Doe",
                            "jane@doe.com"
                        )
                    ]
                )
            );

        // Act
        var exit = await _command.ExecuteInternalAsync(
            CommandContextFactory.Create("list"),
            settings,
            CancellationToken.None
        );

        // Assert
        exit.Should().Be(CliExit.Success());

        await _dispatcher.Received(1).Send(
            Arg.Any<ListProfilesQuery>(),
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnError_WhenQueryFails() {
        // Arrange
        var settings = CommandSettingsFactory.ListProfiles();

        _dispatcher
            .Send(
                Arg.Any<ListProfilesQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                Result<IReadOnlyCollection<ListProfilesResponse>>.Failure(
                    new Error(
                        "Profiles.NotFound",
                        "No profiles found."
                    )
                )
            );

        // Act
        var exit = await _command.ExecuteInternalAsync(
            CommandContextFactory.Create("list"),
            settings,
            CancellationToken.None
        );

        // Assert
        exit.Should().Be(CliExit.Error());

        await _dispatcher.Received(1).Send(
            Arg.Any<ListProfilesQuery>(),
            CancellationToken.None
        );
    }
}
