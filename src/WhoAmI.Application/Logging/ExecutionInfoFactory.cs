using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Abstractions.Serialization;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Logging;

internal sealed class ExecutionInfoFactory : IExecutionInfoFactory {
    private readonly IObjectSerializer _serializer;

    public ExecutionInfoFactory(IObjectSerializer serializer) =>
        _serializer = serializer;

    public ExecutionInfo Create(
        string operation,
        object request,
        ResultBase result,
        long elapsedMilliseconds
    ) =>
        new ExecutionInfo(
            Operation: operation,
            Request: _serializer.Serialize(request),
            Success: result.IsSuccess,
            ElapsedMilliseconds: elapsedMilliseconds,
            Errors: result.IsFailure ? result.Errors : null
        );

    public ExecutionInfo Create(
        string operation,
        object request,
        Exception exception,
        long elapsedMilliseconds
    ) =>
        new ExecutionInfo(
            Operation: operation,
            Request: _serializer.Serialize(request),
            Success: false,
            ElapsedMilliseconds: elapsedMilliseconds,
            Exception: exception
        );
}
