using WhoAmI.Application.Results;

namespace WhoAmI.Application.Logging;

public sealed record ExecutionInfo(
    string Operation,
    object? Request,
    bool Success,
    long ElapsedMilliseconds,
    IReadOnlyCollection<Error>? Errors = null,
    Exception? Exception = null
);
