using Microsoft.Data.Sqlite;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Exceptions;

namespace WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Parsers.Sqlite;

internal sealed class SqliteConstraintViolationParser :
    IConstraintViolationParser
{
    public PersistenceViolationCode Parse(Exception ex) {
        if (ex is not SqliteException sqliteEx) {
            throw new UnrecognizedConstraintException(
                "Expected SqliteException.",
                ex
            );
        }

        return ParseMessage(sqliteEx.Message)
               ?? throw new UnrecognizedConstraintException(
                   $"Constraint could not be mapped from message: " +
                   $"'{sqliteEx.Message}'.",
                   sqliteEx
               );
    }

    internal static PersistenceViolationCode? ParseMessage(string message) {
        foreach (var (signature, code) in SqliteConstraintSignatures.Registry) {
            if (
                message.Contains(signature, StringComparison.OrdinalIgnoreCase)
            ) {
                return code;
            }
        }

        return null;
    }
}
