using System.Diagnostics;
using WhoAmI.Application.Abstractions.Decorators;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Decorators.Queries;

internal sealed class QueryExecutionDecorator<TQuery, TResult> :
    IQueryHandler<TQuery, TResult>,
    IHandlerDecorator<IQueryHandler<TQuery, TResult>>
    where TQuery : IQuery<TResult>
{
    public IQueryHandler<TQuery, TResult> Inner { get; }

    private readonly ISafeExecutionLogger _logger;

    private readonly IExecutionInfoFactory _executionInfo;

    public QueryExecutionDecorator(
        IQueryHandler<TQuery, TResult> inner,
        ISafeExecutionLogger logger,
        IExecutionInfoFactory executionInfo
    ) {
        Inner = inner;
        _logger = logger;
        _executionInfo = executionInfo;
    }

    public async Task<Result<TResult>> HandleAsync(
        TQuery query,
        CancellationToken cancellationToken = default
    ) {
        var stopwatch = Stopwatch.StartNew();

        try {
            var result = await Inner.HandleAsync(query, cancellationToken);

            stopwatch.Stop();

            _logger.Log(
                _executionInfo.Create(
                    typeof(TQuery).Name,
                    query,
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
                    typeof(TQuery).Name,
                    query,
                    ex,
                    stopwatch.ElapsedMilliseconds
                )
            );

            throw;
        }
    }
}
