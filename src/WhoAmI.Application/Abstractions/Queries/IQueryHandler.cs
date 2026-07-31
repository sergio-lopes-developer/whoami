using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Queries;

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<Result<TResult>> HandleAsync(
        TQuery query,
        CancellationToken cancellationToken = default
    );
}
