using WhoAmI.Application.Logging;

namespace WhoAmI.Application.Abstractions.Logging;

internal interface ISafeExecutionLogger {
    void Log(ExecutionInfo info);
}
