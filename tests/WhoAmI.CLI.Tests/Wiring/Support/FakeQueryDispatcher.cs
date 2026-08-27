using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Results;

namespace WhoAmI.CLI.Tests.Wiring.Support;

internal sealed class FakeQueryDispatcher : IQueryDispatcher {
    private readonly List<object> _queries = [];

    private readonly Dictionary<Type, object> _results = [];

    public IReadOnlyList<object> Queries => _queries;

    public object? LastQuery => _queries.LastOrDefault();

    public void SetupResult<TResult>(Result<TResult> result) =>
        _results[typeof(TResult)] = result;

    public Task<Result<TResult>> Send<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    ) {
        _queries.Add(query);

        if (!_results.TryGetValue(typeof(TResult), out var result)) {
            throw new InvalidOperationException(
                "No fake result configured for query result type " +
                $"'{typeof(TResult).Name}'."
            );
        }

        return Task.FromResult((Result<TResult>)result);
    }
}
