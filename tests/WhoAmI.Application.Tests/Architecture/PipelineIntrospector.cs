using WhoAmI.Application.Abstractions.Decorators;

namespace WhoAmI.Application.Tests.Architecture;

internal static class PipelineIntrospector {
    internal static IEnumerable<object> Unwrap(object handler) {
        var current = handler;

        while (true) {
            yield return current;

            var decoratorInterface = current
                .GetType()
                .GetInterfaces()
                .FirstOrDefault(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IHandlerDecorator<>)
                );

            if (decoratorInterface is null) {
                yield break;
            }

            var innerProperty = decoratorInterface.GetProperty(
                nameof(IHandlerDecorator<object>.Inner)
            );

            if (innerProperty is null) {
                yield break;
            }

            var inner = innerProperty.GetValue(current);

            if (inner is null) {
                yield break;
            }

            current = inner;
        }
    }

    internal static string Describe(IEnumerable<object> pipeline) =>
        string.Join(" -> ", pipeline.Select(p => p.GetType().Name));
}
