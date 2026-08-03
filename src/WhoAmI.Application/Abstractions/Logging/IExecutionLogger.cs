using WhoAmI.Application.Logging;

namespace WhoAmI.Application.Abstractions.Logging;

public interface IExecutionLogger {
    void Log(ExecutionInfo info);
}
