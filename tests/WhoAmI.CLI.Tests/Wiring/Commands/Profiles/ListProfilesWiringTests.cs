using FluentAssertions;
using WhoAmI.Application.Features.Profiles.ListProfiles;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public class ListProfilesWiringTests {
    [Fact]
    public void ListProfiles_ShouldResolveAndDispatchQuery_WhenCliCommandIsExecuted() {
        // Arrange
        var response = Array.Empty<ListProfilesResponse>();
        string[] commandLineArguments = ["profile", "list"];

        // Act
        var query = CliApplicationQueryRunner
            .Run<ListProfilesQuery, IReadOnlyCollection<ListProfilesResponse>>(
                response,
                commandLineArguments
            );

        // Assert
        query.Should().NotBeNull();
    }
}
