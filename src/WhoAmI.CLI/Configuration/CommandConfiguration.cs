using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using WhoAmI.CLI.Commands;

namespace WhoAmI.CLI.Configuration;

internal static class CommandConfiguration {
    public static int Run(IHost host, string[] args) {
        var app = new CommandApp();
        app.Configure(ConfigureCommands);

        return app.Run(args);
    }

    private static void ConfigureCommands(IConfigurator config) {
        config.SetApplicationName("WhoAmI CLI");
        config.ValidateExamples();

        config.AddCommand<VersionCommand>("version").WithAlias("v");
    }
}
