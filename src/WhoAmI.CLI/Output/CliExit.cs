using WhoAmI.CLI.Enums;

namespace WhoAmI.CLI.Output;

internal static class CliExit {
    internal static int Error() => (int)ExitCode.Error;

    internal static int Success() => (int)ExitCode.Success;
}
