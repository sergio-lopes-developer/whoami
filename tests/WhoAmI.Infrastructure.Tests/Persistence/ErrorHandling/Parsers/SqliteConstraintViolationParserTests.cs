using FluentAssertions;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Exceptions;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Parsers.Sqlite;
using WhoAmI.Infrastructure.Tests.Persistence.ErrorHandling.TestHelpers;

namespace WhoAmI.Infrastructure.Tests.Persistence.ErrorHandling.Parsers;

public sealed class SqliteConstraintViolationParserTests {
    private readonly SqliteConstraintViolationParser _parser = new();

    [Fact]
    public void Parse_ShouldReturnDuplicateProfileEmail() {
        // Arrange
        var exception = SqliteConstraintExceptionFactory
            .CreateUniqueConstraintViolation("email");

        // Act
        var result = _parser.Parse(exception);

        // Assert
        result.Should().Be(PersistenceViolationCode.DuplicateProfileEmail);
    }

    [Fact]
    public void Parse_ShouldReturnDuplicateProfileGuid() {
        // Arrange
        var exception = SqliteConstraintExceptionFactory
            .CreateUniqueConstraintViolation("id");

        // Act
        var result = _parser.Parse(exception);

        // Assert
        result.Should().Be(PersistenceViolationCode.DuplicateProfileGuid);
    }

    [Fact]
    public void Parse_ShouldThrow_WhenConstraintIsUnknown() {
        // Arrange
        var exception = SqliteConstraintExceptionFactory
            .CreateUniqueConstraintViolation("unknown");

        // Act
        var act = () => _parser.Parse(exception);

        // Assert
        act.Should()
            .Throw<UnrecognizedConstraintException>()
            .WithMessage("*Constraint could not be mapped*");
    }

    [Fact]
    public void Parse_ShouldThrow_WhenExceptionIsNotSqliteException() {
        // Arrange
        var exception = new InvalidOperationException();

        // Act
        var act = () => _parser.Parse(exception);

        // Assert
        act.Should()
            .Throw<UnrecognizedConstraintException>()
            .WithMessage("Expected SqliteException.");
    }

    [Fact]
    public void ParseMessage_ShouldIgnoreCase() {
        // Arrange
        var message =
            "SQLite Error 19: " +
            "'UNIQUE constraint failed: profiles.EMAIL'.";

        // Act
        var result = SqliteConstraintViolationParser.ParseMessage(message);

        // Assert
        result.Should().Be(PersistenceViolationCode.DuplicateProfileEmail);
    }

    [Fact]
    public void ParseMessage_ShouldReturnNull_WhenConstraintIsUnknown() {
        // Arrange
        var message =
            "SQLite Error 19: " +
            "'UNIQUE constraint failed: Profiles.unknown'.";

        // Act
        var result = SqliteConstraintViolationParser.ParseMessage(message);

        // Assert
        result.Should().BeNull();
    }
}
