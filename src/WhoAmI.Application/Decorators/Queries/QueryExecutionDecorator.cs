using WhoAmI.Application.Abstractions.Decorators;
using WhoAmI.Application.Abstractions.Execution;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Decorators.Queries;

internal sealed class QueryExecutionDecorator<TQuery, TResult> :
    IQueryHandler<TQuery, TResult>,
    IHandlerDecorator<IQueryHandler<TQuery, TResult>>
    where TQuery : IQuery<TResult>
{
    public IQueryHandler<TQuery, TResult> Inner { get; }

    private readonly IExecutionRunner _runner;

    public QueryExecutionDecorator(
        IQueryHandler<TQuery, TResult> inner,
        IExecutionRunner runner
    ) {
        Inner = inner;
        _runner = runner;
    }

    public async Task<Result<TResult>> HandleAsync(
        TQuery query,
        CancellationToken cancellationToken = default
    ) =>
        await _runner.ExecuteAsync(
            typeof(TQuery).Name,
            query,
            () => Inner.HandleAsync(query, cancellationToken)
        );
}
