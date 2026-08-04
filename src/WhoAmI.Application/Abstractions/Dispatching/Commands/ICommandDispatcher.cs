using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Dispatching.Commands;

public interface ICommandDispatcher {
    Task<Result> Send(
        ICommand command,
        CancellationToken cancellationToken = default
    );

    Task<Result<TResult>> Send<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    );
}
