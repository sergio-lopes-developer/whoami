using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Decorators;
using WhoAmI.Application.Abstractions.Persistence;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Decorators.Commands;

internal sealed class CommandUnitOfWorkDecorator<TCommand, TResult> :
    ICommandHandler<TCommand, TResult>,
    IHandlerDecorator<ICommandHandler<TCommand, TResult>>
    where TCommand : ICommand<TResult>
{
    public ICommandHandler<TCommand, TResult> Inner { get; }

    private readonly IUnitOfWork _unitOfWork;

    public CommandUnitOfWorkDecorator(
        ICommandHandler<TCommand, TResult> inner,
        IUnitOfWork unitOfWork
    ) {
        Inner = inner;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TResult>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    ) {
        var result = await Inner.HandleAsync(command, cancellationToken);

        if (result.IsFailure) return result;

        var commitResult = await _unitOfWork.CommitAsync(cancellationToken);

        return commitResult.IsSuccess
            ? result
            : Result<TResult>.Failure(commitResult.Errors!);
    }
}
