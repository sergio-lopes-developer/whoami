using Microsoft.Extensions.DependencyInjection;
using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Dispatching.Commands;

internal sealed class CommandDispatcher : ICommandDispatcher {
    private readonly IServiceProvider _provider;

    public CommandDispatcher(IServiceProvider provider) => _provider = provider;

    public async Task<Result> Send(
        ICommand command,
        CancellationToken cancellationToken = default
    ) {
        var adapterType = typeof(CommandHandlerAdapter<>)
            .MakeGenericType(command.GetType());

        var adapter = (ICommandHandlerAdapter)_provider
            .GetRequiredService(adapterType);

        var result = await adapter.HandleAsync(command, cancellationToken);

        return (Result)result!;
    }

    public async Task<Result<TResult>> Send<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    ) {
        var adapterType = typeof(CommandHandlerAdapter<,>)
            .MakeGenericType(command.GetType(), typeof(TResult));

        var adapter = (ICommandHandlerAdapter)_provider
            .GetRequiredService(adapterType);

        var result = await adapter.HandleAsync(command, cancellationToken);

        return (Result<TResult>)result!;
    }
}
