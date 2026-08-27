using WhoAmI.CLI.Hosting;

await using var application = CliApplication.Create(args);

await application.InitializeAsync();

var exitCode = application.Run();

return exitCode;
