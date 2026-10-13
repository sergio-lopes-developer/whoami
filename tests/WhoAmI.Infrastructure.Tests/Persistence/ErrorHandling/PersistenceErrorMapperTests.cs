using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using WhoAmI.Application.Errors;
using WhoAmI.Application.Results;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;

namespace WhoAmI.Infrastructure.Tests.Persistence.ErrorHandling;

public class PersistenceErrorMapperTests {
    [Fact]
    public void Map_ShouldReturnMappedError_WhenMatchingViolationMapperExists() {
        // Arrange
        var parser = Substitute.For<IConstraintViolationParser>();

        parser
            .Parse(Arg.Any<Exception>())
            .Returns(PersistenceViolationCode.DuplicateProfileEmail);

        var expectedError = new Error(
            "Profile.EmailAlreadyExists",
            "The email is already associated with a profile."
        );

        var violationMapper =
            Substitute.For<IPersistenceViolationMapper>();

        violationMapper.Code.Returns(
            PersistenceViolationCode.DuplicateProfileEmail
        );

        violationMapper
            .Map(Arg.Any<DbUpdateException>())
            .Returns(expectedError);

        var errorMapper = new PersistenceErrorMapper(
            parser,
            [violationMapper]
        );

        var exception = new DbUpdateException(
            "Failure",
            new Exception()
        );

        // Act
        var result = errorMapper.Map(exception);

        // Assert
        result.Should().Be(expectedError);
    }

    [Fact]
    public void Map_ShouldReturnUnmappedViolation_WhenMatchingViolationMapperDoesNotExist() {
        // Arrange
        var parser = Substitute.For<IConstraintViolationParser>();

        parser
            .Parse(Arg.Any<Exception>())
            .Returns(new PersistenceViolationCode("Unknown"));

        var errorMapper = new PersistenceErrorMapper(
            parser,
            []
        );

        var exception = new DbUpdateException(
            "Failure",
            new Exception()
        );

        // Act
        var result = errorMapper.Map(exception);

        // Assert
        result.Should().Be(PersistenceErrors.UnmappedViolation);
    }
}
