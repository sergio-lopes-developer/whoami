using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using WhoAmI.CLI.Commands;
using WhoAmI.CLI.DependencyInjection.Spectre;

namespace WhoAmI.CLI.Configuration;

internal static class CommandConfiguration {
    public static int Run(IHost host, string[] args) {
        var registrar = new SpectreCliTypeRegistrar(host.Services);

        var app = new CommandApp(registrar);
        app.Configure(ConfigureCommands);

        return app.Run(args);
    }

    private static void ConfigureCommands(IConfigurator config) {
        config.SetApplicationName("WhoAmI CLI");
        config.ValidateExamples();

        config.AddCommand<VersionCommand>("version").WithAlias("v");
    }
}
