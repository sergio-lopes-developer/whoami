using System.Diagnostics;
using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Decorators;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Decorators.Commands;

internal sealed class CommandExecutionDecorator<TCommand, TResult> :
    ICommandHandler<TCommand, TResult>,
    IHandlerDecorator<ICommandHandler<TCommand, TResult>>
    where TCommand : ICommand<TResult>
{
    public ICommandHandler<TCommand, TResult> Inner { get; }

    private readonly ISafeExecutionLogger _logger;

    private readonly IExecutionInfoFactory _executionInfo;

    public CommandExecutionDecorator(
        ICommandHandler<TCommand, TResult> inner,
        ISafeExecutionLogger logger,
        IExecutionInfoFactory executionInfo
    ) {
        Inner = inner;
        _logger = logger;
        _executionInfo = executionInfo;
    }

    public async Task<Result<TResult>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    ) {
        var stopwatch = Stopwatch.StartNew();

        try {
            var result = await Inner.HandleAsync(command, cancellationToken);

            stopwatch.Stop();

            _logger.Log(
                _executionInfo.Create(
                    typeof(TCommand).Name,
                    command,
                    result,
                    stopwatch.ElapsedMilliseconds
                )
            );

            return result;
        }
        catch (Exception ex) {
            stopwatch.Stop();

            _logger.Log(
                _executionInfo.Create(
                    typeof(TCommand).Name,
                    command,
                    ex,
                    stopwatch.ElapsedMilliseconds
                )
            );

            throw;
        }
    }
}
