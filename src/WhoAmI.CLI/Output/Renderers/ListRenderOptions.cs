using Spectre.Console;

namespace WhoAmI.CLI.Output.Renderers;

internal sealed record ListRenderOptions {
    public required IReadOnlyList<ListColumnDefinition> Columns { get; init; }

    public required IEnumerable<IReadOnlyList<string>> Items { get; init; }

    public HorizontalAlignment Alignment { get; init; }
        = HorizontalAlignment.Center;

    public TableBorder Border { get; init; } = TableBorder.Rounded;
}
