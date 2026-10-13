using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Dispatching.Queries;

internal sealed class QueryHandlerAdapter<TQuery, TResult> :
    IQueryHandlerAdapter<TResult> where TQuery : IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _handler;

    public QueryHandlerAdapter(IQueryHandler<TQuery, TResult> handler) {
        _handler = handler;
    }

    public async Task<Result<TResult>> HandleAsync(
        IQuery<TResult> query,
        CancellationToken cancellationToken
    ) =>
        await _handler.HandleAsync((TQuery)query, cancellationToken);
}
