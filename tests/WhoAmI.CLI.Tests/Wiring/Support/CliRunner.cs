using FluentAssertions;
using WhoAmI.CLI.Output;

namespace WhoAmI.CLI.Tests.Wiring.Support;

internal static class CliRunner {
    public static CliExecutionContext Run(
        CliExecutionContext context,
        params string[] commandLineArguments
    ) {
        // Assert
        context.CommandApp
            .Run(commandLineArguments, TestContext.Current.CancellationToken)
            .Should()
            .Be(CliExit.Success());

        return context;
    }

    public static int Run(params string[] commandLineArguments) {
        var cli = CliExecutionContextFactory.CreateContext();

        return cli.CommandApp.Run(
            commandLineArguments,
            TestContext.Current.CancellationToken
        );
    }
}
