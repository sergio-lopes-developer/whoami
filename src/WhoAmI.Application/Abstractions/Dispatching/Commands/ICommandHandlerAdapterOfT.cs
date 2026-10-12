using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Dispatching.Commands;

internal interface ICommandHandlerAdapter<TResult> {
    Task<Result<TResult>> HandleAsync(
        ICommand<TResult> command,
        CancellationToken cancellationToken
    );
}
