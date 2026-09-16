using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Features.Profiles.ListProfiles;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Settings.Profiles;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands.Profiles;

[Collection("CLI Console")]
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

    private Task<(int ExitCode, string ConsoleOutput)> Execute(
        ListProfilesCommandSettings settings
    ) =>
        CliCommandExecutor.Execute(settings, _command.ExecuteInternalAsync);

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnSuccess_WhenQuerySucceeds() {
        // Arrange
        var settings = CommandSettingsFactory.ListProfiles();

        ListProfilesResponse johnProfile = new(
            "John",
            "Doe",
            "john@doe.com"
        );

        ListProfilesResponse janeProfile = new(
            "Jane",
            "Doe",
            "jane@doe.com"
        );

        _dispatcher
            .Send(
                Arg.Any<ListProfilesQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                Result<IReadOnlyCollection<ListProfilesResponse>>.Success([
                    johnProfile,
                    janeProfile
                ])
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());
        consoleOutput.Should().ContainAll(
            $"{johnProfile.FirstName} {johnProfile.LastName}",
            johnProfile.Email,
            $"{janeProfile.FirstName} {janeProfile.LastName}",
            janeProfile.Email
        );

        await _dispatcher.Received(1).Send(
            Arg.Any<ListProfilesQuery>(),
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldReturnSuccess_WhenNoProfilesWereFound() {
        // Arrange
        var settings = CommandSettingsFactory.ListProfiles();

        _dispatcher
            .Send(
                Arg.Any<ListProfilesQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                Result<IReadOnlyCollection<ListProfilesResponse>>.Success([])
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());
        consoleOutput.Should().ContainAll("No profiles were found.");

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
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Error());
        consoleOutput.Should().Contain("No profiles found.");

        await _dispatcher.Received(1).Send(
            Arg.Any<ListProfilesQuery>(),
            CancellationToken.None
        );
    }
}
