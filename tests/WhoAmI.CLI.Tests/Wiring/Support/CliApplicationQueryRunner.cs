using FluentAssertions;
using WhoAmI.Application.Abstractions.Queries;

namespace WhoAmI.CLI.Tests.Wiring.Support;

internal static class CliApplicationQueryRunner {
    internal static TQuery Run<TQuery, TResponse>(
        TResponse response,
        params string[] commandLineArguments
    ) where TQuery : class, IQuery<TResponse> {
        var context = CliExecutionContextFactory.CreateContext();

        FakeDispatcherSetup.SuccessfulQuery(context, response);

        context = CliRunner.Run(context, commandLineArguments);

        return context.QueryDispatcher.LastQuery
            .Should()
            .BeOfType<TQuery>()
            .Subject;
    }
}
