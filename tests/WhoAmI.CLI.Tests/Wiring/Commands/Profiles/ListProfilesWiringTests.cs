using FluentAssertions;
using WhoAmI.Application.Features.Profiles.ListProfiles;
using WhoAmI.CLI.Tests.Wiring.Support;

namespace WhoAmI.CLI.Tests.Wiring.Commands.Profiles;

public sealed class ListProfilesWiringTests {
    [Fact]
    public void ListProfiles_Should_BeResolvedAndDispatchQuery() {
        // Act
        var query = CliApplicationQueryRunner
            .Run<ListProfilesQuery, IReadOnlyCollection<ListProfilesResponse>>(
                [],
                "profile", "list"
            );

        // Assert
        query.Should().NotBeNull();
    }
}
