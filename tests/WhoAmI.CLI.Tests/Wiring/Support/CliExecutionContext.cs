using Spectre.Console.Cli;

namespace WhoAmI.CLI.Tests.Wiring.Support;

internal sealed class CliExecutionContext {
    public CommandApp CommandApp { get; }

    public FakeCommandDispatcher CommandDispatcher { get; }

    public FakeQueryDispatcher QueryDispatcher { get; }

    public CliExecutionContext(
        CommandApp commandApp,
        FakeCommandDispatcher commandDispatcher,
        FakeQueryDispatcher queryDispatcher
    ) {
        CommandApp = commandApp;
        CommandDispatcher = commandDispatcher;
        QueryDispatcher = queryDispatcher;
    }
}
