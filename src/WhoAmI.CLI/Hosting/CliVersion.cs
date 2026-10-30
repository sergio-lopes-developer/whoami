using System.Reflection;

namespace WhoAmI.CLI.Hosting;

internal static class CliVersion {
    private static string InformationalVersion =>
        typeof(CliVersion)
            .Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion ?? "unknown";

    public static string Current => InformationalVersion.Split('+')[0];

    public static string Commit =>
        InformationalVersion.Contains('+')
            ? InformationalVersion.Split('+')[1]
            : "unknown";
}
