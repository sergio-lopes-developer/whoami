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
        "linkedin",
        "github"
    );

    private static GetProfileByEmailQuery RunGetProfileQuery(
        params string[] args
    ) {
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
        var response = RunGetProfileQuery([
            "--email", email,
            "--hide-email",
            "--hide-linkedin",
            "--verbose"
        ]);

        // Assert
        response.Email.Should().Be(email);
    }
}
