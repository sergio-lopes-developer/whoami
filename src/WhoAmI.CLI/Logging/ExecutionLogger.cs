using Serilog;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Logging;

namespace WhoAmI.CLI.Logging;

internal sealed class ExecutionLogger : IExecutionLogger {
    public void Log(ExecutionInfo info) {
        if (info.Exception != null) {
            LogExecution(info).Error(
                info.Exception,
                "Execution completed with errors."
            );
            return;
        }

        if (!info.Success) {
            LogExecution(info).Warning("Execution completed with warnings");
            return;
        }

        LogExecution(info).Information("Execution completed successfully");
    }

    private static ILogger LogExecution(ExecutionInfo info) =>
        Serilog.Log
            .ForContext("Operation", info.Operation)
            .ForContext("ElapsedMilliseconds", info.ElapsedMilliseconds)
            .ForContext("Request", info.Request, destructureObjects: true)
            .ForContext("Errors", info.Errors, destructureObjects: true);
}
