using FluentAssertions;
using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Output;

namespace WhoAmI.CLI.Tests.Wiring.Support;

internal static class CliCommandRunner {
    private static void SetupSuccessResult<TResponse>(
        CliCommandApp cli,
        TResponse result
    ) =>
        cli.CommandDispatcher.SetupResult(Result<TResponse>.Success(result));

    internal static TCommand Run<TCommand, TResponse>(
        TResponse response,
        params string[] args
    ) where TCommand : class, ICommand<TResponse> {
        var cli = CliCommandAppFactory.Create();

        SetupSuccessResult(cli, response);

        var exit = cli.App.Run([..args], TestContext.Current.CancellationToken);

        // Assert
        exit.Should().Be(CliExit.Success());

        return cli.CommandDispatcher.LastCommand
            .Should()
            .BeOfType<TCommand>()
            .Subject;
    }

    internal static TCommand Run<TCommand>(
        params string[] args
    ) where TCommand : class, ICommand {
        var cli = CliCommandAppFactory.Create();

        var exit = cli.App.Run([..args], TestContext.Current.CancellationToken);

        // Assert
        exit.Should().Be(CliExit.Success());

        return cli.CommandDispatcher.LastCommand
            .Should()
            .BeOfType<TCommand>()
            .Subject;
    }

    internal static int Run(params string[] args) {
        var cli = CliCommandAppFactory.Create();

        return cli.App.Run(args, TestContext.Current.CancellationToken);
    }
}
