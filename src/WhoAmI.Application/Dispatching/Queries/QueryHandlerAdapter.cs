using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Abstractions.Queries;

namespace WhoAmI.Application.Dispatching.Queries;

internal sealed class QueryHandlerAdapter<TQuery, TResult> :
    IQueryHandlerAdapter
    where TQuery : IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _handler;

    public QueryHandlerAdapter(IQueryHandler<TQuery, TResult> handler) =>
        _handler = handler;

    public async Task<object?> HandleAsync(
        object query,
        CancellationToken cancellationToken
    ) =>
        await _handler.HandleAsync((TQuery)query, cancellationToken);
}
