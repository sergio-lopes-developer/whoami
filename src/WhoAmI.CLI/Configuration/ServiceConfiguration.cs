using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WhoAmI.Bootstrap.DependencyInjection;
using WhoAmI.CLI.Commands;

namespace WhoAmI.CLI.Configuration;

internal static class ServiceConfiguration {
    public static void Configure(HostApplicationBuilder builder) {
        builder.Services.AddTransient<VersionCommand>();

        builder.Services.AddBootstrap(
            builder.Configuration,
            builder.Environment
        );
    }
}
