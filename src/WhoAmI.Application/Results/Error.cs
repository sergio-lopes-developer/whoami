namespace WhoAmI.Application.Results;

public sealed record Error(
    string Code,
    string Message,
    IReadOnlyDictionary<string, object?>? Metadata = null
);
