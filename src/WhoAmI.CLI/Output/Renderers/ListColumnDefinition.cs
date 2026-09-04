using Spectre.Console;

namespace WhoAmI.CLI.Output.Renderers;

internal sealed record ListColumnDefinition(
    string Header,
    Justify Alignment = Justify.Left
);
