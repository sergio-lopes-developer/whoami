using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Time;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.Domain.Profiles;

namespace WhoAmI.Application.Tests.Features.Profiles.CreateProfile;

public class CreateProfileCommandHandlerTests {
    [Fact]
    public async Task CreateProfile_ShouldCreateProfile_WhenEmailIsUnique() {
        // Arrange
        var repo =  Substitute.For<IProfileRepository>();

        var createdAt = new DateTimeOffset(
            2026, 9, 22,
            13, 25, 0,
            TimeSpan.Zero
        );

        var clock = Substitute.For<IClock>();
        clock.UtcNow.Returns(createdAt);

        var handler = new CreateProfileCommandHandler(repo, clock);

        var command = new CreateProfileCommand(
            "Sérgio",
            "Lopes",
            "email@example.com",
            "https://www.linkedin.com/in/username",
            "https://github.com/username"
        );

        // Act
        var result = await handler.HandleAsync(
            command,
            TestContext.Current.CancellationToken
        );

        // Assert
        repo.Received(1).Add(
            Arg.Is<Profile>(p =>
                p != null &&
                p.CreatedAt == createdAt &&
                p.Email.Address == command.Email &&
                p.FullName.FirstName.Value == command.FirstName &&
                p.FullName.LastName.Value == command.LastName &&
                p.LinkedIn.Value == command.LinkedIn &&
                p.GitHub.Value == command.GitHub
            )
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBe(Guid.Empty);
        result.Value.FirstName.Should().Be(command.FirstName);
        result.Value.LastName.Should().Be(command.LastName);
        result.Value.Email.Should().Be(command.Email);
        result.Value.LinkedIn.Should().Be(command.LinkedIn);
        result.Value.GitHub.Should().Be(command.GitHub);
    }
}
