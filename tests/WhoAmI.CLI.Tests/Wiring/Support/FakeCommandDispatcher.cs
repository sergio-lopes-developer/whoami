using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Results;

namespace WhoAmI.CLI.Tests.Wiring.Support;

internal sealed class FakeCommandDispatcher : ICommandDispatcher {
    private readonly List<object> _commands = [];

    private readonly Dictionary<Type, object> _results = [];

    public IReadOnlyList<object> Commands => _commands;

    public object? LastCommand => _commands.LastOrDefault();

    public void SetupResult(Result result) => _results[typeof(Result)] = result;

    public void SetupResult<TResult>(Result<TResult> result) =>
        _results[typeof(TResult)] = result;

    public Task<Result> Send(
        ICommand command,
        CancellationToken cancellationToken = default
    ) {
        _commands.Add(command);

        return Task.FromResult(
            _results.TryGetValue(typeof(Result), out var result)
                ? (Result)result
                : Result.Success()
        );
    }

    public Task<Result<TResult>> Send<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    ) {
        _commands.Add(command);

        if (!_results.TryGetValue(typeof(TResult), out var result)) {
            throw new InvalidOperationException(
                $"No fake result configured for {typeof(TResult).Name}.");
        }

        return Task.FromResult((Result<TResult>)result);
    }
}
