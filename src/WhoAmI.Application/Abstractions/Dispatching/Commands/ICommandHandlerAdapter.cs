using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Dispatching.Commands;

internal interface ICommandHandlerAdapter {
    Task<Result> HandleAsync(
        ICommand command,
        CancellationToken cancellationToken
    );
}
