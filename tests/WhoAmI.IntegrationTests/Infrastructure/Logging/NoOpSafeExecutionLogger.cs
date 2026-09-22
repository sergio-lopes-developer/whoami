using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Logging;

namespace WhoAmI.IntegrationTests.Infrastructure.Logging;

internal sealed class NoOpSafeExecutionLogger : ISafeExecutionLogger {
    public void Log(ExecutionInfo info) {
        // intentionally empty
    }
}
