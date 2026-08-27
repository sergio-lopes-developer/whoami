using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Output;
using WhoAmI.Domain.Shared.Exceptions;

namespace WhoAmI.CLI.Commands.Core;

public abstract class CommandBase<TCommand, TSettings>
    : AsyncCommand<TSettings> where TSettings : CommandSettings
{
    private readonly ILogger<TCommand> _logger;

    protected CommandBase(ILogger<TCommand> logger) => _logger = logger;

    protected sealed override async Task<int> ExecuteAsync(
        CommandContext context,
        TSettings settings,
        CancellationToken cancellationToken
    ) {
        try {
            return await ExecuteCommandAsync(
                context,
                settings,
                cancellationToken
            );
        }
        catch (OperationCanceledException) {
            return CliExit.Success();
        }
        catch (DomainException ex) {
            CliMessage.ShowWarning(ex.Message);
            return CliExit.Success();
        }
        catch (Exception ex) {
            _logger.LogError(ex, "CLI unhandled exception");

            CliMessage.ShowError(
                "An unexpected error occurred. See the log for details."
            );

            return CliExit.Error();
        }
    }

    protected abstract Task<int> ExecuteCommandAsync(
        CommandContext context,
        TSettings settings,
        CancellationToken cancellationToken
    );

    protected static int ShowCommandResult(
        ResultBase result,
        string successMessage
    ) {
        if (result.IsSuccess) {
            CliMessage.ShowSuccess(successMessage);
            return CliExit.Success();
        }

        CliMessage.ShowError(
            result.Errors.Select(error => error.Message).ToArray()
        );

        return CliExit.Error();
    }
}
