using Spectre.Console.Cli;

namespace WhoAmI.CLI.DependencyInjection.Spectre;

internal sealed class SpectreCliTypeRegistrar(
    IServiceProvider serviceProvider
) : ITypeRegistrar {
    public ITypeResolver Build() => new SpectreCliTypeResolver(serviceProvider);

    public void Register(Type service, Type implementation) {
        // Not needed since DI is already configured
    }

    public void RegisterInstance(Type service, object implementation) {
        // Not needed
    }

    public void RegisterLazy(Type service, Func<object> factory) {
        // Not needed
    }
}
