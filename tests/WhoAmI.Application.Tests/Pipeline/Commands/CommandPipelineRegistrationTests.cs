using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WhoAmI.Application.Abstractions.Persistence;
using WhoAmI.Application.Decorators.Commands;
using WhoAmI.Application.Features.Profiles;
using WhoAmI.Application.Features.Profiles.UpdateEmail;
using WhoAmI.Application.Results;
using WhoAmI.Application.Tests.Architecture;
using WhoAmI.Application.Tests.TestInfrastructure;

namespace WhoAmI.Application.Tests.Pipeline.Commands;

public class CommandPipelineRegistrationTests {
    private static void AssertPipeline(
        Type handlerInterface,
        IReadOnlyList<object> pipeline
    ) {
        var description = PipelineIntrospector.Describe(pipeline);

        var because =
            $"Handler {handlerInterface.Name} pipeline is invalid.\n" +
            $"Pipeline was: {description}";

        pipeline
            .Should()
            .HaveCount(
                4,
                $"{because}\nExpected exactly 3 decorators and 1 handler."
            );

        IsDecorator(
            pipeline[0],
            typeof(CommandExecutionDecorator<>),
            typeof(CommandExecutionDecorator<,>)
        )
        .Should()
        .BeTrue($"{because}\nExpected Execution as outer decorator.");

        IsDecorator(
            pipeline[1],
            typeof(CommandValidationDecorator<>),
            typeof(CommandValidationDecorator<,>)
        )
        .Should()
        .BeTrue($"{because}\nExpected Validation as outer decorator.");

        IsDecorator(
            pipeline[2],
            typeof(CommandUnitOfWorkDecorator<>),
            typeof(CommandUnitOfWorkDecorator<,>)
        )
        .Should()
        .BeTrue($"{because}\nExpected UnitOfWork as outer decorator.");

        var handler = pipeline[3];

        PipelineAssertions.AssertConcreteHandler(handler, because);
    }

    private static bool IsDecorator(
        object instance,
        params Type[] decoratorTypes
    ) {
        var type = instance.GetType();

        return
            type.IsGenericType &&
            decoratorTypes.Contains(type.GetGenericTypeDefinition());
    }

    [Fact]
    public void Pipeline_ShouldBeConsistent_ForAllHandlers() {
        // Arrange
        var repo = Substitute.For<IProfileRepository>();

        var uow = Substitute.For<IUnitOfWork>();

        uow.CommitAsync(Arg.Any<CancellationToken>()).Returns(Result.Success());

        var provider = CommandPipelineTestFactory.Create(repo, uow);

        var handlerInterfaces =
            HandlerDiscovery.DiscoverCommandHandlerInterfaces(
                typeof(UpdateEmailCommandHandler).Assembly
            );

        // Act + Assert
        foreach (var handlerInterface in handlerInterfaces) {
            var handler = provider.GetRequiredService(handlerInterface);

            var pipeline = PipelineIntrospector.Unwrap(handler).ToList();

            AssertPipeline(handlerInterface, pipeline);
        }
    }
}
