namespace WhoAmI.Application.Abstractions.Dispatching.Queries;

internal interface IQueryHandlerAdapter {
    Task<object?> HandleAsync(
        object query,
        CancellationToken cancellationToken
    );
}
