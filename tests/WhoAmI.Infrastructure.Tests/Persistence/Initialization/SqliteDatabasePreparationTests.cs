using FluentAssertions;
using WhoAmI.Infrastructure.Data.Persistence.Initialization;
using WhoAmI.Infrastructure.Tests.Persistence.TestInfrastructure;

namespace WhoAmI.Infrastructure.Tests.Persistence.Initialization;

public class SqliteDatabasePreparationTests : IClassFixture<TemporaryDirectory> {
    private readonly TemporaryDirectory _directory;

    public SqliteDatabasePreparationTests(TemporaryDirectory directory){
        _directory = directory;
    }

    [Fact]
    public void Prepare_ShouldCreateDatabaseDirectory() {
        // Arrange
        var databasePath = Path.Combine(
            _directory.Path,
            "Data",
            "Database",
            "whoami.db"
        );

        var databaseDirectory = Path.GetDirectoryName(databasePath)!;

        Directory.Exists(databaseDirectory).Should().BeFalse();

        var preparation = new SqliteDatabasePreparation(
            $"Data Source={databasePath}"
        );

        // Act
        preparation.Prepare();

        // Assert
        Directory.Exists(databaseDirectory).Should().BeTrue();
    }

    [Fact]
    public void Prepare_ShouldThrow_WhenDataSourceIsMissing() {
        // Arrange
        var preparation = new SqliteDatabasePreparation("Data Source=");

        // Act
        var act = () => preparation.Prepare();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("The SQLite database path is not configured.");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenConnectionStringIsNull() {
        // Act
        var act = () => new SqliteDatabasePreparation(null!);

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithParameterName("connectionString");
    }
}
