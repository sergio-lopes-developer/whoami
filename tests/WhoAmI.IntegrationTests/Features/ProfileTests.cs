using FluentAssertions;
using WhoAmI.Application.Features.Profiles.CreateProfile;
using WhoAmI.Application.Features.Profiles.GetProfileByEmail;
using WhoAmI.IntegrationTests.Infrastructure;

namespace WhoAmI.IntegrationTests.Features;

public class ProfileTests : IClassFixture<TestApplicationFactory> {
    private readonly TestApplicationFactory _applicationFactory;

    public ProfileTests(TestApplicationFactory applicationFactory) =>
        _applicationFactory = applicationFactory;

    [Fact]
    public async Task CreateProfile_ShouldThrowDuplicateEmailException_WhenEmailAlreadyExists() {
        var scope = await _applicationFactory.CreateTransactionalScopeAsync();

        await using (scope) {
            // Arrange
            var command = new CreateProfileCommand(
                "Sergio",
                "Lopes",
                "email@example.com",
                "https://linkedin.com/in/sergio",
                "https://github.com/sergio"
            );

            await scope.Execute(command, TestContext.Current.CancellationToken);

            // Act
            var result = await scope.Execute(
                command,
                TestContext.Current.CancellationToken
            );

            // Assert
            result.IsFailure.Should().BeTrue();
            result.FirstError!.Code.Should().Be("Profile.EmailAlreadyExists");

            result.FirstError.Metadata!["ProfileEmail"]
                .Should()
                .Be(command.Email);
        }
    }

    [Fact]
    public async Task GetProfileByEmail_ShouldReturnNotFound_WhenProfileDoesNotExist() {
        // Arrange
        const string email = "john.doe@email.com";
        var query = new GetProfileByEmailQuery(email);
        var scope = await _applicationFactory.CreateScopeAsync();

        await using (scope) {
            // Act
            var result = await scope.Query(
                query,
                TestContext.Current.CancellationToken
            );

            // Assert
            result.IsFailure.Should().BeTrue();
            result.FirstError!.Code.Should().Be("Profile.NotFoundByEmail");
            result.FirstError.Metadata!["ProfileEmail"].Should().Be(email);
        }
    }

    [Fact]
    public async Task GetProfileByEmail_ShouldReturnProfile_WhenProfileExists() {
        // Arrange
        const string email = "email@example.com";

        var command = new CreateProfileCommand(
            "Sergio",
            "Lopes",
            email,
            "https://linkedin.com/in/sergio",
            "https://github.com/sergio"
        );

        var commandScope = await _applicationFactory.CreateScopeAsync();

        await using (commandScope) {
            await commandScope.Execute(
                command,
                TestContext.Current.CancellationToken
            );
        }

        var queryScope = await _applicationFactory.CreateScopeAsync();

        await using (queryScope) {
            // Act
            var query = new GetProfileByEmailQuery(email);

            var result = await queryScope.Query(
                query,
                TestContext.Current.CancellationToken
            );

            // Assert
            result.IsSuccess.Should().BeTrue();

            var value = result.GetValueOrThrow();

            value.Email.Should().Be(email);
            value.FirstName.Should().Be("Sergio");
            value.LastName.Should().Be("Lopes");
        }
    }
}
