using Microsoft.Extensions.Hosting;
using Serilog;
using WhoAmI.Bootstrap.Initialization;
using WhoAmI.CLI.Configuration;

namespace WhoAmI.CLI.Hosting;

public sealed class CliApplication : IAsyncDisposable {
    private readonly IHost _host;

    private readonly CliApplicationOptions _options;

    private readonly string[] _args;

    private CliApplication(
        IHost host,
        CliApplicationOptions options,
        string[] args
    ) {
        _host = host;
        _options = options;
        _args = args;
    }

    public static CliApplication Create(
        string[] args,
        CliApplicationOptions? options = null
    ) {
        options ??= new CliApplicationOptions();

        var builder = Host.CreateApplicationBuilder(args);

        if (options.ConfigureLogging) {
            LoggingConfiguration.Configure(builder);
        }

        ServiceConfiguration.Configure(builder);

        return new CliApplication(builder.Build(), options, args);
    }

    public async Task InitializeAsync() {
        if (_options.InitializeDatabase) {
            await _host.Services.InitializeDatabaseAsync();
        }
    }

    public int Run() => CommandConfiguration.Run(_host, _args);

    public async ValueTask DisposeAsync() {
        try {
            _host.Dispose();
        }
        finally {
            await Log.CloseAndFlushAsync();
        }
    }
}
