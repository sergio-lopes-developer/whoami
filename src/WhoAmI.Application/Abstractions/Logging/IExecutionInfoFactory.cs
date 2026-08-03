using WhoAmI.Application.Logging;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Logging;

internal interface IExecutionInfoFactory {
    ExecutionInfo Create(
        string operation,
        object request,
        ResultBase result,
        long elapsedMilliseconds
    );

    ExecutionInfo Create(
        string operation,
        object request,
        Exception exception,
        long elapsedMilliseconds
    );
}
