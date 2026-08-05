using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Dispatching.Queries;

public interface IQueryDispatcher {
    Task<Result<TResult>> Send<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    );
}
