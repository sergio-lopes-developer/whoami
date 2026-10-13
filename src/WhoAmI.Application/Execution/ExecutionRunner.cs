using System.Diagnostics;
using WhoAmI.Application.Abstractions.Execution;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Execution;

internal sealed class ExecutionRunner : IExecutionRunner {
    private readonly ISafeExecutionLogger _logger;

    private readonly IExecutionInfoFactory _executionInfo;

    public ExecutionRunner(
        ISafeExecutionLogger logger,
        IExecutionInfoFactory executionInfo
    ) {
        _logger = logger;
        _executionInfo = executionInfo;
    }

    public async Task<TResult> ExecuteAsync<TResult>(
        string operation,
        object request,
        Func<Task<TResult>> handler
    ) where TResult : ResultBase {
        var stopwatch = Stopwatch.StartNew();

        try {
            var result = await handler();

            stopwatch.Stop();

            _logger.Log(
                _executionInfo.Create(
                    operation,
                    request,
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
                    operation,
                    request,
                    ex,
                    stopwatch.ElapsedMilliseconds
                )
            );

            throw;
        }
    }
}
