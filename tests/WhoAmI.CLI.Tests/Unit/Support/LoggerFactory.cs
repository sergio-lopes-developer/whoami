using Microsoft.Extensions.Logging;
using NSubstitute;

namespace WhoAmI.CLI.Tests.Unit.Support;

internal static class LoggerFactory {
    public static ILogger<T> Create<T>() => Substitute.For<ILogger<T>>();
}
