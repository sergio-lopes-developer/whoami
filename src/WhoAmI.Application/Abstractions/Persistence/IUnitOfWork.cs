using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Persistence;

public interface IUnitOfWork {
    Task<Result> CommitAsync(CancellationToken cancellationToken = default);
}
