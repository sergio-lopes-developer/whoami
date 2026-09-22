using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace WhoAmI.IntegrationTests.Infrastructure;

internal sealed class TestHostEnvironment : IHostEnvironment {
    public string EnvironmentName { get; set; } = Environments.Development;

    public string ApplicationName { get; set; } = "WhoAmI.Tests";

    public string ContentRootPath { get; set; } =
        Directory.GetCurrentDirectory();

    public IFileProvider ContentRootFileProvider { get; set; } =
        new NullFileProvider();
}
