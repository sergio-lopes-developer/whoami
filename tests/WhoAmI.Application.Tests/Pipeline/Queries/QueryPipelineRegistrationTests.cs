using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using WhoAmI.Application.Decorators.Queries;
using WhoAmI.Application.Tests.Architecture;
using WhoAmI.Application.Tests.TestInfrastructure;
using WhoAmI.Infrastructure.Data.Queries.Abstractions;
using WhoAmI.Infrastructure.Data.Queries.Features.Profiles.GetProfileByEmail;

namespace WhoAmI.Application.Tests.Pipeline.Queries;

public class QueryPipelineRegistrationTests {
    private static void AssertPipeline(
        Type handlerInterface,
        IReadOnlyList<object> pipeline
    ) {
        var description = PipelineIntrospector.Describe(pipeline);

        var because =
            $"Handler {handlerInterface.Name} pipeline is invalid.\n" +
            $"Pipeline was: {description}";

        pipeline.Should().HaveCount(
            3,
            $"{because}\nExpected exactly 2 decorators and 1 handler."
        );

        IsDecorator(pipeline[0], typeof(QueryExecutionDecorator<,>))
            .Should()
            .BeTrue($"{because}\nExpected Execution as inner decorator.");

        IsDecorator(pipeline[1], typeof(QueryValidationDecorator<,>))
            .Should()
            .BeTrue($"{because}\nExpected Validation as outer decorator.");

        var handler = pipeline[2];

        PipelineAssertions.AssertConcreteHandler(handler, because);
    }

    private static bool IsDecorator(object instance, Type decoratorType) {
        var type = instance.GetType();

        return
            type.IsGenericType &&
            type.GetGenericTypeDefinition() == decoratorType;
    }

    [Fact]
    public void Pipeline_ShouldHave_TwoDecorators() {
        // Arrange
        var connectionFactory = Substitute.For<IDbConnectionFactory>();

        var provider = QueryPipelineTestFactory.Create(connectionFactory);

        var handlerInterfaces =
            HandlerDiscovery.DiscoverQueryHandlerInterfaces(
                typeof(GetProfileByEmailQueryHandler).Assembly
            );

        // Act + Assert
        foreach (var handlerInterface in handlerInterfaces) {
            var handler = provider.GetRequiredService(handlerInterface);

            var pipeline = PipelineIntrospector.Unwrap(handler).ToList();

            AssertPipeline(handlerInterface, pipeline);
        }
    }
}
