using FluentAssertions;
using WhoAmI.Application.Abstractions.Decorators;

namespace WhoAmI.Application.Tests.Pipeline;

internal static class PipelineAssertions {
    public static void AssertConcreteHandler(object handler, string because) {
        handler.GetType().GetInterfaces().Should().NotContain(
            x => x.IsGenericType &&
                 x.GetGenericTypeDefinition() == typeof(IHandlerDecorator<>),
            $"{because}\nFinal element should be the concrete handler."
        );
    }
}
