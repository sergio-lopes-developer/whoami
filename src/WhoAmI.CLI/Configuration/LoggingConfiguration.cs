using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Formatting.Compact;

namespace WhoAmI.CLI.Configuration;

internal static class LoggingConfiguration {
    public static void Configure(HostApplicationBuilder builder) {
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithSensitiveDataMasking(
                options => {
                    options.Mode = MaskingMode.Globally;
                }
            )
            .MinimumLevel.Information()

            // EF Core
            .MinimumLevel.Override(
                "Microsoft.EntityFrameworkCore",
                Serilog.Events.LogEventLevel.Fatal
            )

            // SQL commands
            .MinimumLevel.Override(
                "Microsoft.EntityFrameworkCore.Database.Command",
                Serilog.Events.LogEventLevel.Error
            )

            .WriteTo.File(
                new RenderedCompactJsonFormatter(),
                "logs/log-.json",
                rollingInterval: RollingInterval.Day
            )

            .WriteTo.File(
                "logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} " +
                "[{Level:u3}] {Message:lj} | {Properties} " +
                "{NewLine}{Exception}"
            )

            .CreateLogger();

        builder.Services.AddSerilog(Log.Logger);
    }
}
