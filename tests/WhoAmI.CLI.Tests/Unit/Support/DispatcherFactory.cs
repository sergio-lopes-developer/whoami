using NSubstitute;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Abstractions.Dispatching.Queries;

namespace WhoAmI.CLI.Tests.Unit.Support;

internal static class DispatcherFactory {
    public static ICommandDispatcher CreateCommandDispatcher()
        => Substitute.For<ICommandDispatcher>();

    public static IQueryDispatcher CreateQueryDispatcher()
        => Substitute.For<IQueryDispatcher>();
}
