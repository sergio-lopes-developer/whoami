using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using WhoAmI.CLI.Commands;
using WhoAmI.CLI.DependencyInjection.Spectre;

namespace WhoAmI.CLI.Configuration;

internal static class CommandConfiguration {
    internal static CommandApp Create(ITypeRegistrar registrar) {
        var app = new CommandApp(registrar);

        app.Configure(Configure);

        return app;
    }

    internal static int Run(IHost host, string[] args) {
        var registrar = new SpectreCliTypeRegistrar(host.Services);

        return Create(registrar).Run(args);
    }

    private static void Configure(IConfigurator config) {
        config.SetApplicationName("WhoAmI CLI");
        config.ValidateExamples();

        config.AddCommand<VersionCommand>("version").WithAlias("v");
    }
}
