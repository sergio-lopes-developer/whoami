using FluentAssertions;
using WhoAmI.Application.Validation;
using WhoAmI.Domain.Shared.Constraints;

namespace WhoAmI.Application.Tests.Validation;

public class ValidationRuleBuilderTests {
    private static readonly TextLengthConstraint _lengthConstraint =
        new(
            2,
            10,
            value =>
                !string.IsNullOrWhiteSpace(value) &&
                value.Length >= 2 &&
                value.Length <= 10
        );

    [Fact]
    public void IsRequired_ShouldAddError_WhenStringIsNull() {
        // Act
        var errors = Ensure.Field<string?>(null, "Name")
            .IsRequired()
            .ToErrors();

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.Required");
    }

    [Fact]
    public void IsRequired_ShouldAddError_WhenStringIsEmpty() {
        // Act
        var errors = Ensure.Field("", "Name")
            .IsRequired()
            .ToErrors();

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }

    [Fact]
    public void IsRequired_ShouldAddError_WhenStringIsWhitespace() {
        // Act
        var errors = Ensure.Field("   ", "Name")
            .IsRequired()
            .ToErrors();

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }

    [Fact]
    public void IsRequired_ShouldAddError_WhenGuidIsEmpty() {
        // Act
        var errors = Ensure.Field(Guid.Empty, "Id")
            .IsRequired()
            .ToErrors();

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }

    [Fact]
    public void IsRequired_ShouldNotAddError_WhenValueIsValid() {
        // Act
        var errors = Ensure.Field("John", "Name")
            .IsRequired()
            .ToErrors();

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Satisfies_ShouldAddError_WhenPredicateFails() {
        // Act
        var errors = Ensure.Field("invalid", "Email")
            .Satisfies(email => email == "valid@email.com")
            .ToErrors();

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.InvalidFormat");
    }

    [Fact]
    public void Satisfies_ShouldNotAddError_WhenPredicateSucceeds() {
        // Act
        var errors = Ensure.Field("valid@email.com", "Email")
            .Satisfies(email => email == "valid@email.com")
            .ToErrors();

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Satisfies_ShouldIgnoreNullValue() {
        // Act
        var errors = Ensure.Field<string?>(null, "Email")
            .Satisfies(_ => false)
            .ToErrors();

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Satisfies_ShouldIgnoreWhitespaceValue() {
        // Act
        var errors = Ensure.Field("   ", "Email")
            .Satisfies(_ => false)
            .ToErrors();

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void HasValidLength_ShouldAddError_WhenTooShort() {
        // Act
        var errors = Ensure.Field("A", "Name")
            .HasValidLength(_lengthConstraint)
            .ToErrors();

        // Assert
        errors.Should().ContainSingle();

        var error = errors.Single();

        error.Code.Should().Be("Validation.InvalidLength");
    }

    [Fact]
    public void HasValidLength_ShouldAddError_WhenTooLong() {
        // Act
        var errors = Ensure.Field("ABCDEFGHIJK", "Name")
            .HasValidLength(_lengthConstraint)
            .ToErrors();

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.InvalidLength");
    }

    [Fact]
    public void HasValidLength_ShouldNotAddError_WhenLengthIsValid() {
        // Act
        var errors = Ensure.Field("John", "Name")
            .HasValidLength(_lengthConstraint)
            .ToErrors();

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void HasValidLength_ShouldIgnoreNullValue() {
        // Act
        var errors = Ensure.Field<string?>(null, "Name")
            .HasValidLength(_lengthConstraint)
            .ToErrors();

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void HasValidLength_ShouldIgnoreWhitespaceValue() {
        // Act
        var errors = Ensure.Field("   ", "Name")
            .HasValidLength(_lengthConstraint)
            .ToErrors();

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void AddTo_ShouldCopyErrorsToTargetCollection() {
        // Arrange
        var errors = new List<WhoAmI.Application.Results.Error>();

        // Act
        Ensure.Field("", "Name")
            .IsRequired()
            .AddTo(errors);

        // Assert
        errors.Should().ContainSingle();

        errors.Single().Code.Should().Be("Validation.Required");
    }

    [Fact]
    public void AddTo_ShouldDoNothing_WhenNoErrorsExist() {
        // Arrange
        var errors = new List<WhoAmI.Application.Results.Error>();

        // Act
        Ensure.Field("John", "Name")
            .IsRequired()
            .AddTo(errors);

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void ValidationMethods_ShouldAccumulateMultipleErrors() {
        // Act
        var errors = Ensure.Field("A", "Name")
            .HasValidLength(_lengthConstraint)
            .Satisfies(value => value == "John")
            .ToErrors();

        // Assert
        errors.Should().HaveCount(2);

        errors.Select(e => e.Code).Should().Contain(
            "Validation.InvalidLength"
        );

        errors.Select(e => e.Code).Should().Contain(
            "Validation.InvalidFormat"
        );
    }
}
