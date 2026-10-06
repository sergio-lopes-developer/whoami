using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.Output;
using WhoAmI.CLI.Settings.Profiles;
using WhoAmI.CLI.Tests.Unit.Support;

namespace WhoAmI.CLI.Tests.Unit.Commands.Profiles;

[Collection("CLI Console")]
public sealed class GetProfileUnitTests {
    private const string Id = "b30545b1-b92b-4040-bbca-c733deb8b1c2";
    private const string FirstName = "John";
    private const string LastName = "Doe";
    private const string LinkedIn = "linkedin";
    private const string GitHub = "github";

    private static string FormatDate(DateTimeOffset? date) =>
        date?.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss zzz") ?? "Never";

    private static readonly DateTimeOffset _createdAt =
        new(2026, 9, 22, 13, 25, 0, TimeSpan.Zero);

    private static Result<GetProfileByEmailResponse> GenerateResponse(
        string email,
        DateTimeOffset? updatedAt = null
    ) =>
        Result<GetProfileByEmailResponse>.Success(
            new GetProfileByEmailResponse(
                Guid.Parse(Id),
                FirstName,
                LastName,
                email,
                LinkedIn,
                GitHub,
                _createdAt,
                updatedAt
            )
        );

    private readonly IQueryDispatcher _dispatcher =
        DispatcherFactory.CreateQueryDispatcher();

    private readonly GetProfile _command;

    public GetProfileUnitTests() {
        _command = new GetProfile(
            _dispatcher,
            LoggerFactory.Create<GetProfile>()
        );
    }

    private Task<(int ExitCode, string ConsoleOutput)> Execute(
        GetProfileCommandSettings settings
    ) =>
        CliCommandExecutor.Execute(settings, _command.ExecuteInternalAsync);

    private static void ShouldNotContainVerboseInformation(
        string consoleOutput
    ) {
        consoleOutput.Should().NotContain("ID:");
        consoleOutput.Should().NotContain(Id);
        consoleOutput.Should().NotContain("Created at:");
        consoleOutput.Should().NotContain("Updated at:");
    }

    private async Task ShouldHaveDispatchedQuery(string email) {
        await _dispatcher.Received(1).Send(
            Arg.Is<GetProfileByEmailQuery>(q => q.Email == email),
            Arg.Any<CancellationToken>()
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
                GenerateResponse(settings.Email)
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());

        ShouldNotContainVerboseInformation(consoleOutput);

        consoleOutput.Should().Contain($"{FirstName} {LastName}");
        consoleOutput.Should().Contain("Email:");
        consoleOutput.Should().Contain(settings.Email);
        consoleOutput.Should().Contain("GitHub:");
        consoleOutput.Should().Contain(GitHub);
        consoleOutput.Should().Contain("LinkedIn:");
        consoleOutput.Should().Contain(LinkedIn);

        await ShouldHaveDispatchedQuery(settings.Email);
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldHideEmail_WhenRequested() {
        // Arrange
        var settings = CommandSettingsFactory.GetProfile(
            hideEmail: true
        );

        _dispatcher
            .Send(
                Arg.Any<GetProfileByEmailQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                GenerateResponse(settings.Email)
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());

        consoleOutput.Should().NotContain("Email:");
        consoleOutput.Should().NotContain(settings.Email);

        ShouldNotContainVerboseInformation(consoleOutput);

        consoleOutput.Should().Contain($"{FirstName} {LastName}");
        consoleOutput.Should().Contain("GitHub:");
        consoleOutput.Should().Contain(GitHub);
        consoleOutput.Should().Contain("LinkedIn:");
        consoleOutput.Should().Contain(LinkedIn);

        await ShouldHaveDispatchedQuery(settings.Email);
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldHideGitHub_WhenRequested() {
        // Arrange
        var settings = CommandSettingsFactory.GetProfile(
            hideGitHub: true
        );

        _dispatcher
            .Send(
                Arg.Any<GetProfileByEmailQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                GenerateResponse(settings.Email)
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());

        consoleOutput.Should().NotContain("GitHub:");
        consoleOutput.Should().NotContain(GitHub);

        ShouldNotContainVerboseInformation(consoleOutput);

        consoleOutput.Should().Contain($"{FirstName} {LastName}");
        consoleOutput.Should().Contain("Email:");
        consoleOutput.Should().Contain(settings.Email);
        consoleOutput.Should().Contain("LinkedIn:");
        consoleOutput.Should().Contain(LinkedIn);

        await ShouldHaveDispatchedQuery(settings.Email);
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldHideLinkedIn_WhenRequested() {
        // Arrange
        var settings = CommandSettingsFactory.GetProfile(
            hideLinkedIn: true
        );

        _dispatcher
            .Send(
                Arg.Any<GetProfileByEmailQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                GenerateResponse(settings.Email)
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());

        consoleOutput.Should().NotContain("LinkedIn:");
        consoleOutput.Should().NotContain(LinkedIn);

        ShouldNotContainVerboseInformation(consoleOutput);

        consoleOutput.Should().Contain($"{FirstName} {LastName}");
        consoleOutput.Should().Contain("Email:");
        consoleOutput.Should().Contain(settings.Email);
        consoleOutput.Should().Contain("GitHub:");
        consoleOutput.Should().Contain(GitHub);

        await ShouldHaveDispatchedQuery(settings.Email);
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldDisplayVerboseInformation_WhenRequested() {
        // Arrange
        var settings = CommandSettingsFactory.GetProfile(
            verbose: true
        );

        var updatedAt = new DateTimeOffset(
            2026, 9, 25,
            18, 55, 0,
            TimeSpan.Zero
        );

        _dispatcher
            .Send(
                Arg.Any<GetProfileByEmailQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                GenerateResponse(settings.Email, updatedAt)
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());

        consoleOutput.Should().Contain("ID:");
        consoleOutput.Should().Contain(Id);
        consoleOutput.Should().Contain("Created at:");
        consoleOutput.Should().Contain(FormatDate(_createdAt));
        consoleOutput.Should().Contain("Updated at:");
        consoleOutput.Should().Contain(FormatDate(updatedAt));
        consoleOutput.Should().Contain($"{FirstName} {LastName}");
        consoleOutput.Should().Contain("Email:");
        consoleOutput.Should().Contain(settings.Email);
        consoleOutput.Should().Contain("GitHub:");
        consoleOutput.Should().Contain(GitHub);
        consoleOutput.Should().Contain("LinkedIn:");
        consoleOutput.Should().Contain(LinkedIn);

        await ShouldHaveDispatchedQuery(settings.Email);
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldDisplayNever_WhenVerboseRequestedForProfileWithoutUpdateAtDate() {
        // Arrange
        var settings = CommandSettingsFactory.GetProfile(
            verbose: true
        );

        _dispatcher
            .Send(
                Arg.Any<GetProfileByEmailQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                GenerateResponse(settings.Email)
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());

        consoleOutput.Should().Contain("ID:");
        consoleOutput.Should().Contain(Id);
        consoleOutput.Should().Contain("Created at:");
        consoleOutput.Should().Contain(FormatDate(_createdAt));
        consoleOutput.Should().Contain("Updated at:");
        consoleOutput.Should().Contain("Never");
        consoleOutput.Should().Contain($"{FirstName} {LastName}");
        consoleOutput.Should().Contain("Email:");
        consoleOutput.Should().Contain(settings.Email);
        consoleOutput.Should().Contain("GitHub:");
        consoleOutput.Should().Contain(GitHub);
        consoleOutput.Should().Contain("LinkedIn:");
        consoleOutput.Should().Contain(LinkedIn);

        await ShouldHaveDispatchedQuery(settings.Email);
    }

    [Fact]
    public async Task ExecuteInternalAsync_ShouldDisplayWarning_WhenAllOptionalInformationIsHidden() {
        // Arrange
        var settings = CommandSettingsFactory.GetProfile(
            hideEmail: true,
            hideGitHub: true,
            hideLinkedIn: true
        );

        _dispatcher
            .Send(
                Arg.Any<GetProfileByEmailQuery>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(
                GenerateResponse(settings.Email)
            );

        // Act
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Success());

        consoleOutput.Should().Contain("All profile information is hidden.");
        consoleOutput.Should()
            .Contain("Use --verbose to display additional information.");

        consoleOutput.Should().NotContain($"{FirstName} {LastName}");
        consoleOutput.Should().NotContain("Email:");
        consoleOutput.Should().NotContain(settings.Email);
        consoleOutput.Should().NotContain("GitHub:");
        consoleOutput.Should().NotContain(GitHub);
        consoleOutput.Should().NotContain("LinkedIn:");
        consoleOutput.Should().NotContain(LinkedIn);

        await ShouldHaveDispatchedQuery(settings.Email);
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
        var (exitCode, consoleOutput) = await Execute(settings);

        // Assert
        exitCode.Should().Be(CliExit.Error());
        consoleOutput.Should().Contain("Profile not found.");

        await ShouldHaveDispatchedQuery(settings.Email);
    }
}
