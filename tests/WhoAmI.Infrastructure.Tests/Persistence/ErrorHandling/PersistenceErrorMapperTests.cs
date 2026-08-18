using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using WhoAmI.Application.Errors;
using WhoAmI.Application.Results;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;

namespace WhoAmI.Infrastructure.Tests.Persistence.ErrorHandling;

public sealed class PersistenceErrorMapperTests {
    [Fact]
    public void Map_ShouldReturnMappedError_WhenViolationMapperExists() {
        // Arrange
        var parser = Substitute.For<IConstraintViolationParser>();

        parser.Parse(Arg.Any<Exception>())
            .Returns(PersistenceViolationCode.DuplicateProfileEmail);

        var expectedError = new Error(
            "Profile.DuplicateEmail",
            "Email already exists."
        );

        var violationMapper =
            Substitute.For<IPersistenceViolationMapper>();

        violationMapper.Code.Returns(
            PersistenceViolationCode.DuplicateProfileEmail
        );

        violationMapper
            .Map(Arg.Any<DbUpdateException>())
            .Returns(expectedError);

        var sut = new PersistenceErrorMapper(
            parser,
            [violationMapper]
        );

        var exception = new DbUpdateException(
            "Failure",
            new Exception()
        );

        // Act
        var result = sut.Map(exception);

        // Assert
        result.Should().Be(expectedError);
    }

    [Fact]
    public void Map_ShouldReturnUnmappedViolation_WhenMapperDoesNotExist() {
        // Arrange
        var parser = Substitute.For<IConstraintViolationParser>();

        parser.Parse(Arg.Any<Exception>())
            .Returns(new PersistenceViolationCode("Unknown"));

        var sut = new PersistenceErrorMapper(
            parser,
            []
        );

        var exception = new DbUpdateException(
            "Failure",
            new Exception()
        );

        // Act
        var result = sut.Map(exception);

        // Assert
        result.Should().Be(PersistenceErrors.UnmappedViolation);
    }
}
