using Spectre.Console;

namespace WhoAmI.CLI.Output.Renderers;

internal static class InfoRenderer {
    public static void Render(InfoRenderOptions options) {
        var grid = new Grid();

        grid.AddColumn(new GridColumn().NoWrap());
        grid.AddColumn();

        foreach (var item in options.Items) {
            grid.AddRow($"[bold]{item.Label}[/]", item.DisplayValue);
        }

        var panel = new Panel(grid)
            .Header($"[bold] {options.Title} [/]", options.HeaderAlignment)
            .Border(options.Border);

        AnsiConsole.Write(
            new Align(panel, options.Alignment)
        );
    }
}
