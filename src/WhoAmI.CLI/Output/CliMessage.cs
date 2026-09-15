using Spectre.Console;
using WhoAmI.CLI.Enums;

namespace WhoAmI.CLI.Output;

internal static class CliMessage {
    internal static void ShowError(string message) =>
        Show(message, MessageColor.Red);

    internal static void ShowError(IReadOnlyCollection<string> messages) =>
        Show(string.Join(Environment.NewLine, messages), MessageColor.Red);

    internal static void ShowInfo(string message) =>
        AnsiConsole.MarkupLine(Markup.Escape(message));

    internal static void ShowSuccess(string message) =>
        Show(message, MessageColor.Green);

    internal static void ShowWarning(string message) =>
        Show(message, MessageColor.Yellow);

    private static void Show(string message, MessageColor color) {
        var strColor = color.ToString().ToLowerInvariant();
        AnsiConsole.MarkupLine($"[{strColor}]{Markup.Escape(message)}[/]");
    }
}
