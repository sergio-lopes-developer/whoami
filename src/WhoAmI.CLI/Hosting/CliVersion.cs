using System.Reflection;

namespace WhoAmI.CLI.Hosting;

internal static class CliVersion {
    public static string Current =>
        typeof(CliVersion)
            .Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion
                ?? "unknown";
}
