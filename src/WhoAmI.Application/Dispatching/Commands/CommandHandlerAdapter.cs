using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Dispatching.Commands;

namespace WhoAmI.Application.Dispatching.Commands;

internal sealed class CommandHandlerAdapter<TCommand> :
    ICommandHandlerAdapter where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;

    public CommandHandlerAdapter(ICommandHandler<TCommand> handler) =>
        _handler = handler;

    public async Task<object?> HandleAsync(
        object command,
        CancellationToken cancellationToken
    ) =>
        await _handler.HandleAsync((TCommand)command, cancellationToken);
}
