using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Serialization;
using WhoAmI.Application.Logging;
using WhoAmI.Application.Results;

namespace WhoAmI.Application.Tests.Logging;

public class ExecutionInfoFactoryTests {
    public sealed record FakeRequest;

    public sealed record FakeResult;

    [Fact]
    public void Create_ShouldReturnSuccess_WhenResultIsSuccess() {
        // Arrange
        var serializer = Substitute.For<IObjectSerializer>();
        serializer.Serialize(Arg.Any<object>()).Returns("serialized");

        var factory = new ExecutionInfoFactory(serializer);

        var result = Result.Success();

        // Act
        var info = factory.Create(
            "UpdateEmail",
            new FakeRequest(),
            result,
            123
        );

        // Assert
        info.Operation.Should().Be("UpdateEmail");
        info.Request.Should().Be("serialized");
        info.Success.Should().BeTrue();
        info.ElapsedMilliseconds.Should().Be(123);
        info.Errors.Should().BeNull();
        info.Exception.Should().BeNull();
    }

    [Fact]
    public void Create_ShouldReturnErrorCodes_WhenResultIsFailure() {
        // Arrange
        var serializer = Substitute.For<IObjectSerializer>();
        serializer.Serialize(Arg.Any<object>()).Returns("serialized");

        var factory = new ExecutionInfoFactory(serializer);

        var error = new Error("EmailNotFound", "error");

        var result = Result.Failure(error);

        // Act
        var info = factory.Create(
            "UpdateEmail",
            new FakeRequest(),
            result,
            10
        );

        // Assert
        info.Operation.Should().Be("UpdateEmail");
        info.Request.Should().Be("serialized");
        info.Success.Should().BeFalse();
        info.ElapsedMilliseconds.Should().Be(10);
        info.Errors.Should().Equal([error]);
        info.Exception.Should().BeNull();
    }

    [Fact]
    public void Create_ShouldReturnSuccess_WhenGenericResultIsSuccess() {
        // Arrange
        var serializer = Substitute.For<IObjectSerializer>();
        serializer.Serialize(Arg.Any<object>()).Returns("serialized");

        var factory = new ExecutionInfoFactory(serializer);

        var result = Result<FakeResult>.Success(new FakeResult());

        // Act
        var info = factory.Create(
            "UpdateEmail",
            new FakeRequest(),
            result,
            50
        );

        // Assert
        info.Operation.Should().Be("UpdateEmail");
        info.Request.Should().Be("serialized");
        info.Success.Should().BeTrue();
        info.ElapsedMilliseconds.Should().Be(50);
        info.Errors.Should().BeNull();
        info.Exception.Should().BeNull();
    }

    [Fact]
    public void Create_ShouldReturnErrorCodes_WhenGenericResultIsFailure() {
        // Arrange
        var serializer = Substitute.For<IObjectSerializer>();
        serializer.Serialize(Arg.Any<object>()).Returns("serialized");

        var factory = new ExecutionInfoFactory(serializer);

        var error = new Error("EmailNotFound", "error");

        var result = Result<FakeResult>.Failure(error);

        // Act
        var info = factory.Create(
            "UpdateEmail",
            new FakeRequest(),
            result,
            50
        );

        // Assert
        info.Operation.Should().Be("UpdateEmail");
        info.Request.Should().Be("serialized");
        info.Success.Should().BeFalse();
        info.ElapsedMilliseconds.Should().Be(50);
        info.Errors.Should().Equal([error]);
        info.Exception.Should().BeNull();
    }

    [Fact]
    public void Create_ShouldSetException_WhenExceptionIsProvided() {
        // Arrange
        var serializer = Substitute.For<IObjectSerializer>();
        serializer.Serialize(Arg.Any<object>()).Returns("serialized");

        var factory = new ExecutionInfoFactory(serializer);

        var ex = new InvalidOperationException("boom");

        // Act
        var info = factory.Create(
            "UpdateEmail",
            new FakeRequest(),
            ex,
            99
        );

        // Assert
        info.Operation.Should().Be("UpdateEmail");
        info.Request.Should().Be("serialized");
        info.Success.Should().BeFalse();
        info.ElapsedMilliseconds.Should().Be(99);
        info.Exception.Should().Be(ex);
        info.Errors.Should().BeNull();
    }

    [Fact]
    public void Create_ShouldUseSerializer_ForRequest() {
        // Arrange
        var serializer = Substitute.For<IObjectSerializer>();
        serializer.Serialize(Arg.Any<object>()).Returns("JSON");

        var factory = new ExecutionInfoFactory(serializer);

        var request = new FakeRequest();

        // Act
        factory.Create(
            "UpdateEmail",
            request,
            Result.Success(),
            1
        );

        // Assert
        serializer.Received(1).Serialize(request);
    }
}
