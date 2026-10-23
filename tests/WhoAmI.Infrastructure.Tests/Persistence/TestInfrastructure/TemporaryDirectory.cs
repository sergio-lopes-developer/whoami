namespace WhoAmI.Infrastructure.Tests.Persistence.TestInfrastructure;

public sealed class TemporaryDirectory : IDisposable {
    public string Path { get; }

    public TemporaryDirectory() {
        Path = System.IO.Path.Combine(
            System.IO.Path.GetTempPath(),
            Guid.NewGuid().ToString("N")
        );

        Directory.CreateDirectory(Path);
    }

    public void Dispose() {
        if (Directory.Exists(Path)) {
            Directory.Delete(Path, recursive: true);
        }
    }
}
