using Spectre.Console.Cli;

namespace WhoAmI.CLI.Tests.Wiring.Support;

internal sealed class CliCommandApp {
    public CommandApp App { get; init; }

    public FakeCommandDispatcher CommandDispatcher { get; init; }

    public FakeQueryDispatcher QueryDispatcher { get; init;  }

    public CliCommandApp(
        CommandApp app,
        FakeCommandDispatcher commandDispatcher,
        FakeQueryDispatcher queryDispatcher
    ) {
        App = app;
        CommandDispatcher = commandDispatcher;
        QueryDispatcher = queryDispatcher;
    }
}
