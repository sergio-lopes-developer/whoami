using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Decorators;
using WhoAmI.Application.Abstractions.Execution;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Decorators.Commands;

internal sealed class CommandExecutionDecorator<TCommand> :
    ICommandHandler<TCommand>,
    IHandlerDecorator<ICommandHandler<TCommand>>
    where TCommand : ICommand
{
    public ICommandHandler<TCommand> Inner { get; }

    private readonly IExecutionRunner _runner;

    public CommandExecutionDecorator(
        ICommandHandler<TCommand> inner,
        IExecutionRunner runner
    ) {
        Inner = inner;
        _runner = runner;
    }

    public async Task<Result> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    ) =>
        await _runner.ExecuteAsync(
            typeof(TCommand).Name,
            command,
            () => Inner.HandleAsync(command, cancellationToken)
        );
}

internal sealed class CommandExecutionDecorator<TCommand, TResult> :
    ICommandHandler<TCommand, TResult>,
    IHandlerDecorator<ICommandHandler<TCommand, TResult>>
    where TCommand : ICommand<TResult>
{
    public ICommandHandler<TCommand, TResult> Inner { get; }

    private readonly IExecutionRunner _runner;

    public CommandExecutionDecorator(
        ICommandHandler<TCommand, TResult> inner,
        IExecutionRunner runner
    ) {
        Inner = inner;
        _runner = runner;
    }

    public async Task<Result<TResult>> HandleAsync(
        TCommand command,
        CancellationToken cancellationToken = default
    ) =>
        await _runner.ExecuteAsync(
            typeof(TCommand).Name,
            command,
            () => Inner.HandleAsync(command, cancellationToken)
        );
}
