using Microsoft.Extensions.Hosting;
using WhoAmI.Bootstrap.Initialization;
using WhoAmI.CLI.Configuration;

var builder = Host.CreateApplicationBuilder(args);

ServiceConfiguration.Configure(builder);

using var host = builder.Build();

await host.Services.InitializeDatabaseAsync();

var exitCode = CommandConfiguration.Run(host, args);

return exitCode;
