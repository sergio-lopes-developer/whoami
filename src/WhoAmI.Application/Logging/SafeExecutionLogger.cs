using System.Diagnostics;
using WhoAmI.Application.Abstractions.Logging;

namespace WhoAmI.Application.Logging;

internal sealed class SafeExecutionLogger : ISafeExecutionLogger {
    private readonly IExecutionLogger _logger;

    public SafeExecutionLogger(IExecutionLogger logger) => _logger = logger;

    public void Log(ExecutionInfo info) {
        try {
            _logger.Log(info);
        }
        catch (Exception ex) {
            Debug.WriteLine(ex);
        }
    }
}
