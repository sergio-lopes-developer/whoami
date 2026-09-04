using Spectre.Console;

namespace WhoAmI.CLI.Output.Renderers;

internal static class ListRenderer {
    public static void Render(ListRenderOptions options) {
        var table = new Table();

        table.Border(options.Border);

        foreach (var column in options.Columns) {
            table.AddColumn(
                new TableColumn($"[bold]{column.Header}[/]") {
                    Alignment = column.Alignment
                });
        }

        foreach (var row in options.Items) {
            table.AddRow([.. row]);
        }

        AnsiConsole.Write(
            new Align(table, options.Alignment)
        );
    }
}
