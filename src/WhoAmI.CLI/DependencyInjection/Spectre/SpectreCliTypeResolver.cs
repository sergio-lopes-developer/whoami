using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace WhoAmI.CLI.DependencyInjection.Spectre;

internal sealed class SpectreCliTypeResolver : ITypeResolver, IDisposable {
    private readonly IServiceScope _scope;

    public SpectreCliTypeResolver(IServiceProvider serviceProvider) =>
        _scope = serviceProvider.CreateScope();

    public object Resolve(Type? type) {
        if (type is null) {
            throw new InvalidOperationException();
        }

        if (!typeof(CommandSettings).IsAssignableFrom(type)) {
            return _scope.ServiceProvider.GetRequiredService(type);
        }

        var instance = Activator.CreateInstance(type);

        if (instance is null) {
            throw new InvalidOperationException(
                $"Could not create instance of {type.Name}"
            );
        }

        return instance;
    }

    public void Dispose() => _scope.Dispose();
}
