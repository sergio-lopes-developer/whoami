using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Decorators;
using WhoAmI.Application.Abstractions.Persistence;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Decorators.Commands;

internal sealed class CommandUnitOfWorkDecorator<TCommand> :
    ICommandHandler<TCommand>,
    IHandlerDecorator<ICommandHandler<TCommand>>
    where TCommand : ICommand
{
    private readonly IUnitOfWork _unitOfWork;

    public ICommandHandler<TCommand> Inner { get; }

    public CommandUnitOfWorkDecorator(
        ICommandHandler<TCommand> inner,
        IUnitOfWork unitOfWork
    ) {
        Inner = inner;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    ) {
        var result = await Inner.HandleAsync(command, cancellationToken);

        if (result.IsFailure) return result;

        var commitResult = await _unitOfWork.CommitAsync(cancellationToken);

        return commitResult;
    }
}
