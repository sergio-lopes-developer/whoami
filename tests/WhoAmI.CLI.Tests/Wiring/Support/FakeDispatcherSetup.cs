using WhoAmI.Application.Results;

namespace WhoAmI.CLI.Tests.Wiring.Support;

internal static class FakeDispatcherSetup {
    public static void SuccessfulCommand<TResponse>(
        CliExecutionContext context,
        TResponse result
    ) =>
        context.CommandDispatcher.SetupResult(
            Result<TResponse>.Success(result)
        );

    public static void SuccessfulQuery<TResponse>(
        CliExecutionContext context,
        TResponse result
    ) =>
        context.QueryDispatcher.SetupResult(
            Result<TResponse>.Success(result)
        );
}
