using WhoAmI.CLI.Hosting;

await using var application = CliApplication.Create(args);
await application.InitializeAsync();

return application.Run();
