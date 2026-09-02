using Spectre.Console;

namespace WhoAmI.CLI.Output.Renderers;

internal sealed record InfoRenderOptions {
    public required string Title { get; init; }

    public required IEnumerable<InfoItem> Items { get; init; }

    public Justify HeaderAlignment { get; init; } = Justify.Center;

    public BoxBorder Border { get; init; } = BoxBorder.Rounded;
}
