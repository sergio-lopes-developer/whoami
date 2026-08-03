using FluentAssertions;
using NSubstitute;
using WhoAmI.Application.Abstractions.Logging;
using WhoAmI.Application.Logging;

namespace WhoAmI.Application.Tests.Logging;

public class SafeExecutionLoggerTests {
    [Fact]
    public void Log_ShouldCallInnerLogger() {
        // Arrange
        var logger = Substitute.For<IExecutionLogger>();

        var safeLogger = new SafeExecutionLogger(logger);

        var info = new ExecutionInfo(
            Operation: "UpdateEmail",
            Request: "{}",
            Success: true,
            ElapsedMilliseconds: 1
        );

        // Act
        safeLogger.Log(info);

        // Assert
        logger.Received(1).Log(info);
    }

    [Fact]
    public void Log_ShouldNotThrow_WhenInnerLoggerThrows() {
        // Arrange
        var logger = Substitute.For<IExecutionLogger>();

        logger
            .When(x => x.Log(Arg.Any<ExecutionInfo>()))
            .Do(_ => throw new InvalidOperationException());

        var safeLogger = new SafeExecutionLogger(logger);

        var info = new ExecutionInfo(
            Operation: "UpdateEmail",
            Request: "{}",
            Success: true,
            ElapsedMilliseconds: 1
        );

        // Act
        var act = () => safeLogger.Log(info);

        // Assert
        act.Should().NotThrow();
    }
}
