using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WhoAmI.Bootstrap.DependencyInjection;
using WhoAmI.CLI.Commands;
using WhoAmI.CLI.Commands.Profiles;
using WhoAmI.CLI.DependencyInjection;

namespace WhoAmI.CLI.Configuration;

internal static class ServiceConfiguration {
    public static void Configure(HostApplicationBuilder builder) {
        RegisterCliServices(builder.Services);

        RegisterApplicationServices(
            builder.Services,
            builder.Configuration,
            builder.Environment
        );
    }

    internal static void RegisterCliServices(IServiceCollection services) {
        services
            .AddTransient<CreateProfile>()
            .AddTransient<GetProfile>()
            .AddTransient<UpdateEmail>()
            .AddTransient<UpdateFullName>()
            .AddTransient<UpdateSocialLinks>()
            .AddTransient<VersionCommand>();

        services.AddObservability();
    }

    private static void RegisterApplicationServices(
        IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
    ) =>
        services.AddBootstrap(configuration, environment);
}
