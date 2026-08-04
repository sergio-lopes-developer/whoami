using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Dispatching.Commands;

namespace WhoAmI.Application.Dispatching.Commands;

internal sealed class CommandHandlerAdapter<TCommand, TResult> :
    ICommandHandlerAdapter where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _handler;

    public CommandHandlerAdapter(ICommandHandler<TCommand, TResult> handler) =>
        _handler = handler;

    public async Task<object?> HandleAsync(
        object command,
        CancellationToken cancellationToken
    ) =>
        await _handler.HandleAsync((TCommand)command, cancellationToken);
}
