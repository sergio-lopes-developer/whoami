using Microsoft.Extensions.DependencyInjection;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.CLI.Configuration;
using WhoAmI.CLI.DependencyInjection.Spectre;

namespace WhoAmI.CLI.Tests.Wiring.Support;

internal static class CliCommandAppFactory {
    public static CliCommandApp Create() {
        var services = new ServiceCollection();

        var commandDispatcher = new FakeCommandDispatcher();
        var queryDispatcher = new FakeQueryDispatcher();

        services.AddSingleton(commandDispatcher);
        services.AddSingleton(queryDispatcher);

        services.AddSingleton<ICommandDispatcher>(commandDispatcher);
        services.AddSingleton<IQueryDispatcher>(queryDispatcher);

        services.AddLogging();

        ServiceConfiguration.RegisterCliServices(services);

        var provider = services.BuildServiceProvider();

        var registrar = new SpectreCliTypeRegistrar(provider);

        var app = CommandConfiguration.Create(registrar);

        return new CliCommandApp(app, commandDispatcher, queryDispatcher);
    }
}
