using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Dispatching.Commands;

internal sealed class CommandHandlerAdapter<TCommand> :
    ICommandHandlerAdapter where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;

    public CommandHandlerAdapter(ICommandHandler<TCommand> handler) {
        _handler = handler;
    }

    public async Task<Result> HandleAsync(
        ICommand command,
        CancellationToken cancellationToken
    ) =>
        await _handler.HandleAsync((TCommand)command, cancellationToken);
}

internal sealed class CommandHandlerAdapter<TCommand, TResult> :
    ICommandHandlerAdapter<TResult> where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _handler;

    public CommandHandlerAdapter(ICommandHandler<TCommand, TResult> handler) {
        _handler = handler;
    }

    public async Task<Result<TResult>> HandleAsync(
        ICommand<TResult> command,
        CancellationToken cancellationToken
    ) =>
        await _handler.HandleAsync((TCommand)command, cancellationToken);
}
