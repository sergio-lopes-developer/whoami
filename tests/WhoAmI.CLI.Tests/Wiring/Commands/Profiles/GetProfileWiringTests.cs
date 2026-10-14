using FluentAssertions;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public class GetProfileWiringTests {
    private static GetProfileByEmailResponse GetProfileResponse() => new(
        Guid.NewGuid(),
        "John",
        "Doe",
        "john@doe.com",
        "https://www.linkedin.com/in/username",
        "https://github.com/username",
        new DateTimeOffset(2026, 9, 22, 13, 25, 0, TimeSpan.Zero),
        null
    );

    private static GetProfileByEmailQuery RunGetProfile(
        params string[] commandOptions
    ) {
        string[] commandLineArguments = ["profile", "get", ..commandOptions];

        return CliApplicationQueryRunner
            .Run<GetProfileByEmailQuery, GetProfileByEmailResponse>(
                GetProfileResponse(),
                commandLineArguments
            );
    }

    [Fact]
    public void GetProfile_ShouldBindOptionsAndDispatchQuery_WhenOptionsAreProvided() {
        // Arrange
        const string email = "john@doe.com";

        // Act
        var query = RunGetProfile(["--email", email]);

        // Assert
        query.Email.Should().Be(email);
    }
}
