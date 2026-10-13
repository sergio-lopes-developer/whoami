using Microsoft.Extensions.DependencyInjection;
using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Dispatching.Queries;

internal sealed class QueryDispatcher : IQueryDispatcher {
    private readonly IServiceProvider _provider;

    public QueryDispatcher(IServiceProvider provider) {
        _provider = provider;
    }

    public async Task<Result<TResult>> Send<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    ) {
        var adapterType = typeof(QueryHandlerAdapter<,>)
            .MakeGenericType(query.GetType(), typeof(TResult));

        var adapter = (IQueryHandlerAdapter<TResult>)_provider
            .GetRequiredService(adapterType);

        return await adapter.HandleAsync(query, cancellationToken);
    }
}
