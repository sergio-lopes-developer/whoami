using Microsoft.Data.Sqlite;

namespace WhoAmI.Infrastructure.Tests.Persistence.ErrorHandling.TestHelpers;

internal static class SqliteConstraintExceptionFactory {
    public static SqliteException CreateUniqueConstraintViolation(
        string columnName
    ) {
        using var connection = new SqliteConnection("Data Source=:memory:");

        connection.Open();

        using var create = connection.CreateCommand();

        create.CommandText =
            $"""CREATE TABLE Profiles ({columnName} TEXT UNIQUE);""";

        create.ExecuteNonQuery();

        using var insert1 = connection.CreateCommand();

        insert1.CommandText =
            $"""INSERT INTO Profiles ({columnName}) VALUES ('value1');""";

        insert1.ExecuteNonQuery();

        using var insert2 = connection.CreateCommand();

        insert2.CommandText =
            $"""INSERT INTO Profiles ({columnName}) VALUES ('value1');""";

        try {
            insert2.ExecuteNonQuery();

            throw new InvalidOperationException(
                "Expected SqliteException was not thrown."
            );
        }
        catch (SqliteException ex) {
            return ex;
        }
    }
}
