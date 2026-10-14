using FluentAssertions;
using WhoAmI.Application.Abstractions.Commands;

namespace WhoAmI.CLI.Tests.Wiring.Support;

internal static class CliApplicationCommandRunner {
    internal static TCommand Run<TCommand, TResponse>(
        TResponse response,
        params string[] commandLineArguments
    ) where TCommand : class, ICommand<TResponse> {
        var context = CliExecutionContextFactory.CreateContext();

        FakeDispatcherSetup.SuccessfulCommand(context, response);

        context = CliRunner.Run(context, commandLineArguments);

        return context.CommandDispatcher.LastCommand
            .Should()
            .BeOfType<TCommand>()
            .Subject;
    }

    internal static TCommand Run<TCommand>(
        params string[] commandLineArguments
    ) where TCommand : class, ICommand {
        var context = CliExecutionContextFactory.CreateContext();

        context = CliRunner.Run(context, commandLineArguments);

        return context.CommandDispatcher.LastCommand
            .Should()
            .BeOfType<TCommand>()
            .Subject;
    }
}
