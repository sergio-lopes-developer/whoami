using System.Reflection;
using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Decorators;
using WhoAmI.Application.Abstractions.Queries;

namespace WhoAmI.Application.Tests.Architecture;

internal static class HandlerDiscovery{
    internal static IEnumerable<Type> DiscoverCommandHandlerInterfaces(
        Assembly assembly
    ) =>
        Discover(assembly, IsCommandHandlerInterface);

    internal static IEnumerable<Type> DiscoverQueryHandlerInterfaces(
        Assembly assembly
    ) =>
        Discover(assembly, IsQueryHandlerInterface);

    private static IEnumerable<Type> Discover(
        Assembly assembly,
        Func<Type, bool> predicate
    ) =>
        assembly
            .GetTypes()
            .Where(IsConcreteType)
            .Where(type => !IsDecorator(type))
            .SelectMany(type => type.GetInterfaces())
            .Where(predicate)
            .Distinct();

    private static bool IsConcreteType(Type type) =>
        type is {
            IsAbstract: false,
            IsInterface: false,
            IsGenericTypeDefinition: false
        };

    private static bool IsDecorator(Type type) =>
        type.GetInterfaces().Any(i =>
            i.IsGenericType &&
            i.GetGenericTypeDefinition() ==
            typeof(IHandlerDecorator<>)
        );

    private static bool IsGenericInterface(
        Type type,
        params Type[] definitions
    ) {
        if (!type.IsGenericType) return false;

        var definition = type.GetGenericTypeDefinition();

        return definitions.Contains(definition);
    }

    private static bool IsCommandHandlerInterface(Type type) =>
        IsGenericInterface(
            type,
            typeof(ICommandHandler<>),
            typeof(ICommandHandler<,>)
        );

    private static bool IsQueryHandlerInterface(Type type) =>
        IsGenericInterface(
            type,
            typeof(IQueryHandler<,>)
        );
}
