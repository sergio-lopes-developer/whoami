using FluentAssertions;
using WhoAmI.CLI.Output;

namespace WhoAmI.CLI.Tests.Wiring.Support;

internal static class CliRunner {
    public static CliExecutionContext Run(
        CliExecutionContext context,
        params string[] args
    ) {
        // Assert
        context.CommandApp.Run(args, TestContext.Current.CancellationToken)
            .Should()
            .Be(CliExit.Success());

        return context;
    }

    public static int Run(params string[] args) {
        var cli = CliExecutionContextFactory.CreateContext();

        return cli.CommandApp.Run(args, TestContext.Current.CancellationToken);
    }
}
