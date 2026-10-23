using WhoAmI.CLI.Hosting;
using WhoAmI.CLI.Output;

try {
    await using var application = CliApplication.Create(args);
    await application.InitializeAsync();

    return application.Run();
}
catch (Exception ex) {
    CliMessage.ShowError(ex.Message);
    return CliExit.Error();
}
