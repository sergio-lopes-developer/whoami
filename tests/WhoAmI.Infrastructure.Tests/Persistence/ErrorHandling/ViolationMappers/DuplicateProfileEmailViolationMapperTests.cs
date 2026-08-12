using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Infrastructure.Data.Persistence.Context;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.Abstractions;
using WhoAmI.Infrastructure.Data.Persistence.ErrorHandling.ViolationMappers;
using WhoAmI.Infrastructure.Tests.Persistence.TestInfrastructure;
using WhoAmI.Testing.Factories;

namespace WhoAmI.Infrastructure.Tests.Persistence.ErrorHandling
    .ViolationMappers;

public sealed class DuplicateProfileEmailViolationMapperTests {
    [Fact]
    public void Code_ShouldReturnDuplicateProfileEmail() {
        // Arrange
        var sut = new DuplicateProfileEmailViolationMapper();

        // Act
        var result = sut.Code;

        // Assert
        result.Should().Be(PersistenceViolationCode.DuplicateProfileEmail);
    }

    [Fact]
    public async Task Map_ShouldReturnDuplicateEmailError() {
        var mapper = Substitute.For<IPersistenceErrorMapper>();

        await using var scope =
            new SqliteContextScope<WhoAmIDbContext>(
                options => new WhoAmIDbContext(options, mapper)
            );

        var context = scope.Context;

        var profile = ProfileFactory.Create();

        context.Profiles.Add(profile);

        var entry = context.Entry(profile);

        var exception = new DbUpdateException(
            "Failure",
            new Exception(),
            [entry]
        );

        var sut = new DuplicateProfileEmailViolationMapper();

        // Act
        var result = sut.Map(exception);

        // Assert
        result.Should().BeEquivalentTo(
            ProfileErrors.DuplicateEmail(profile.Email)
        );
    }
}
