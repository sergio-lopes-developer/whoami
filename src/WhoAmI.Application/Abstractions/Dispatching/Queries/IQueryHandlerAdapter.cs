using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Dispatching.Queries;

internal interface IQueryHandlerAdapter<TResult> {
    Task<Result<TResult>> HandleAsync(
        IQuery<TResult> query,
        CancellationToken cancellationToken
    );
}
