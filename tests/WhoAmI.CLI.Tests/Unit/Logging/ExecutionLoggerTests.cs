using FluentAssertions;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.InMemory.Assertions;
using WhoAmI.Application.Logging;
using WhoAmI.Application.Results;
using WhoAmI.CLI.Logging;

namespace WhoAmI.CLI.Tests.Unit.Logging;

public sealed class ExecutionLoggerTests {
    private readonly ExecutionLogger _logger = new();

    public ExecutionLoggerTests() {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.InMemory()
            .CreateLogger();

        InMemorySink.Instance.Dispose();
    }

    private static void AssertCommonProperties(
        LogEvent logEvent,
        ExecutionInfo info
    ) {
        logEvent.Properties.Should().ContainKeys(
            "Operation",
            "ElapsedMilliseconds",
            "Request",
            "Errors"
        );

        logEvent.Properties["Operation"]
            .ToString()
            .Should()
            .Be($"\"{info.Operation}\"");

        logEvent.Properties["ElapsedMilliseconds"]
            .ToString()
            .Should()
            .Be(info.ElapsedMilliseconds.ToString());
    }

    [Fact]
    public void Log_ShouldWriteInformation_WhenExecutionSucceeded() {
        // Arrange
        var info = new ExecutionInfo (
            Success: true,
            ElapsedMilliseconds: 10,
            Errors: null,
            Exception: null,
            Operation: "Operation",
            Request: null
        );

        // Act
        _logger.Log(info);

        // Assert
        InMemorySink.Instance.Should()
            .HaveMessage("Execution completed successfully")
            .Appearing()
            .Once()
            .WithLevel(LogEventLevel.Information);

        InMemorySink.Instance.LogEvents.Should()
            .ContainSingle()
            .Which.Properties.Should()
            .ContainKeys(
                "Operation",
                "ElapsedMilliseconds",
                "Request",
                "Errors"
            );

        AssertCommonProperties(
            InMemorySink.Instance.LogEvents.Should().ContainSingle().Subject,
            info
        );
    }

    [Fact]
    public void Log_ShouldWriteWarning_WhenExecutionFailedWithoutException() {
        // Arrange
        var info = new ExecutionInfo(
            Success: false,
            ElapsedMilliseconds: 10,
            Errors: null,
            Exception: null,
            Operation: "Operation",
            Request: null
        );

        // Act
        _logger.Log(info);

        // Assert
        InMemorySink.Instance.Should()
            .HaveMessage("Execution completed with warnings")
            .Appearing()
            .Once()
            .WithLevel(LogEventLevel.Warning);

        AssertCommonProperties(
            InMemorySink.Instance.LogEvents.Should().ContainSingle().Subject,
            info
        );
    }

    [Fact]
    public void Log_ShouldWriteError_WhenExecutionHasException() {
        // Arrange
        var exception = new InvalidOperationException("Boom");

        var info = new ExecutionInfo(
            Success: false,
            ElapsedMilliseconds: 10,
            Errors: null,
            Exception: exception,
            Operation: "Operation",
            Request: null
        );

        // Act
        _logger.Log(info);

        // Assert
        InMemorySink.Instance.Should()
            .HaveMessage("Execution completed with errors.")
            .Appearing()
            .Once()
            .WithLevel(LogEventLevel.Error);

        AssertCommonProperties(
            InMemorySink.Instance.LogEvents.Should().ContainSingle().Subject,
            info
        );
    }

    [Fact]
    public void Log_ShouldDestructureRequest_WhenRequestIsProvided() {
        // Arrange
        var request = new {
            Id = Guid.NewGuid(),
            Email = "john@example.com"
        };

        var info = new ExecutionInfo(
            Success: true,
            ElapsedMilliseconds: 25,
            Errors: null,
            Exception: null,
            Operation: "UpdateEmail",
            Request: request
        );

        // Act
        _logger.Log(info);

        // Assert
        var logEvent = InMemorySink.Instance.LogEvents
            .Should()
            .ContainSingle()
            .Subject;

        logEvent.Properties["Request"]
            .Should()
            .BeOfType<StructureValue>();
    }

    [Fact]
    public void Log_ShouldIncludeErrors_WhenErrorsAreProvided() {
        // Arrange
        var error = new Error (
            Code: "EMAIL_INVALID",
            Message: "Email is invalid."
        );

        var info = new ExecutionInfo(
            Success: false,
            ElapsedMilliseconds: 15,
            Errors: [error],
            Exception: null,
            Operation: "UpdateEmail",
            Request: null
        );

        // Act
        _logger.Log(info);

        // Assert
        var logEvent = InMemorySink.Instance.LogEvents
            .Should()
            .ContainSingle()
            .Subject;

        logEvent.Properties["Errors"]
            .Should()
            .BeOfType<SequenceValue>();
    }
}
