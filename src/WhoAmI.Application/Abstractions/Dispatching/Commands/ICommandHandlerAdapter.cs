namespace WhoAmI.Application.Abstractions.Dispatching.Commands;

internal interface ICommandHandlerAdapter {
    Task<object?> HandleAsync(
        object command,
        CancellationToken cancellationToken
    );
}
