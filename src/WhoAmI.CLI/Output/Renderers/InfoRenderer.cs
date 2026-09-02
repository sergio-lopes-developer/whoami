using Spectre.Console;

namespace WhoAmI.CLI.Output.Renderers;

internal static class InfoRenderer {
    public static void Render(string title, IEnumerable<InfoItem> items) {
        var grid = new Grid();

        grid.AddColumn(new GridColumn().NoWrap());
        grid.AddColumn();

        foreach (var item in items) {
            AddRow(grid, item);
        }

        var panel = new Panel(grid)
            .Header($"[bold] {title} [/]", Justify.Center)
            .Border(BoxBorder.Rounded);

        AnsiConsole.Write(Align.Center(panel));
    }

    private static void AddRow(Grid grid, InfoItem item) =>
        grid.AddRow(
            $"[bold]{item.Label}[/]",
            item.Value ?? "-"
        );
}
