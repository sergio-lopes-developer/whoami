using NSubstitute;
using Spectre.Console.Cli;

namespace WhoAmI.CLI.Tests.Unit.Support;

internal static class CommandContextFactory {
    public static CommandContext Create(string name = "command") =>
        new([], Substitute.For<IRemainingArguments>(), name, null);
}
