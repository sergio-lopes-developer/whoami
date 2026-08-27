namespace WhoAmI.CLI.Hosting;

public sealed class CliApplicationOptions {
    public bool InitializeDatabase { get; init; } = true;

    public bool ConfigureLogging { get; init; } = true;
}
