using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Decorators.Queries;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Tests.Decorators.Queries;

public class QueryValidationDecoratorTests {
    public sealed record FakeQuery(string Value) : IQuery<FakeResult>;

    public sealed record FakeResult;

    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenValidationFails() {
        // Arrange
        var query = new FakeQuery("invalid");

        var error = new Error(
            "Validation.Required",
            "Value is required."
        );

        var validator = Substitute.For<IQueryValidator<FakeQuery>>();

        validator.Validate(query).Returns([error]);

        var inner = Substitute.For<IQueryHandler<FakeQuery, FakeResult>>();

        var decorator = new QueryValidationDecorator<
            FakeQuery,
            FakeResult
        >(
            inner,
            [validator]
        );

        // Act
        var result = await decorator.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.FirstError.Should().Be(error);

        await inner.DidNotReceive()
            .HandleAsync(Arg.Any<FakeQuery>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldInvokeInnerHandler_WhenValidationSucceeds() {
        // Arrange
        var query = new FakeQuery("valid");

        var expected = new FakeResult();

        var validator = Substitute.For<IQueryValidator<FakeQuery>>();

        validator.Validate(query).Returns([]);

        var inner = Substitute.For<IQueryHandler<FakeQuery, FakeResult>>();

        inner.HandleAsync(query, Arg.Any<CancellationToken>())
            .Returns(Result<FakeResult>.Success(expected));

        var decorator = new QueryValidationDecorator<
            FakeQuery,
            FakeResult
        >(
            inner,
            [validator]
        );

        // Act
        var result = await decorator.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);

        await inner.Received(1)
            .HandleAsync(query, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldAggregateErrors_FromAllValidators() {
        // Arrange
        var query = new FakeQuery("invalid");

        var error1 = new Error(
            "Validation.Required",
            "Value is required."
        );

        var error2 = new Error(
            "Validation.InvalidFormat",
            "Value format is invalid."
        );

        var validator1 = Substitute.For<IQueryValidator<FakeQuery>>();

        validator1.Validate(query).Returns([error1]);

        var validator2 = Substitute.For<IQueryValidator<FakeQuery>>();

        validator2.Validate(query).Returns([error2]);

        var inner = Substitute.For<IQueryHandler<FakeQuery, FakeResult>>();

        var decorator = new QueryValidationDecorator<
            FakeQuery,
            FakeResult
        >(
            inner,
            [validator1, validator2]
        );

        // Act
        var result = await decorator.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Errors.Should().HaveCount(2);

        result.Errors.Should().Contain(error1);
        result.Errors.Should().Contain(error2);

        await inner.DidNotReceive()
            .HandleAsync(Arg.Any<FakeQuery>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldRemoveEquivalentErrors() {
        // Arrange
        var query = new FakeQuery("invalid");

        var error1 = new Error(
            "Validation.Required",
            "Value is required."
        );

        var error2 = new Error(
            "Validation.Required",
            "Value is required."
        );

        var validator1 = Substitute.For<IQueryValidator<FakeQuery>>();

        validator1.Validate(query).Returns([error1]);

        var validator2 = Substitute.For<IQueryValidator<FakeQuery>>();

        validator2.Validate(query).Returns([error2]);

        var inner = Substitute.For<IQueryHandler<FakeQuery, FakeResult>>();

        var decorator = new QueryValidationDecorator<
            FakeQuery,
            FakeResult
        >(inner, [validator1, validator2]);

        // Act
        var result = await decorator.HandleAsync(
            query,
            TestContext.Current.CancellationToken
        );

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Errors.Should().ContainSingle();

        await inner.DidNotReceive()
            .HandleAsync(Arg.Any<FakeQuery>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldPropagateException_WhenInnerThrows() {
        // Arrange
        var query = new FakeQuery("valid");

        var validator = Substitute.For<IQueryValidator<FakeQuery>>();

        validator.Validate(query).Returns([]);

        var inner = Substitute.For<IQueryHandler<FakeQuery, FakeResult>>();

        inner.HandleAsync(query, Arg.Any<CancellationToken>())
            .Returns(
                Task.FromException<Result<FakeResult>>(
                    new InvalidOperationException()
                )
            );

        var decorator = new QueryValidationDecorator<
            FakeQuery,
            FakeResult
        >(
            inner,
            [validator]
        );

        // Act
        var act = () => decorator.HandleAsync(query);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
