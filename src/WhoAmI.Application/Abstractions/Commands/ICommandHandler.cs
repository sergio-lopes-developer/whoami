using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Commands;

public interface ICommandHandler<in TCommand> where TCommand : ICommand{
    Task<Result> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    );
}
