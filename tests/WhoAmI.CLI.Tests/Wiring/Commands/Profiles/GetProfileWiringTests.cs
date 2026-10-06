using FluentAssertions;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public sealed class GetProfileWiringTests {
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

    private static GetProfileByEmailQuery RunGetProfile(params string[] args) {
        string[] commandArgs = ["profile", "get", ..args];

        return CliApplicationQueryRunner
            .Run<GetProfileByEmailQuery, GetProfileByEmailResponse>(
                GetProfileResponse(),
                commandArgs
            );
    }

    [Fact]
    public void GetProfile_Should_BindCliArgumentsAndDispatchQuery() {
        // Arrange
        const string email = "john@doe.com";

        // Act
        var query = RunGetProfile(["--email", email]);

        // Assert
        query.Email.Should().Be(email);
    }
}
