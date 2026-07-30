using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Commands;

public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<Result<TResult>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    );
}
