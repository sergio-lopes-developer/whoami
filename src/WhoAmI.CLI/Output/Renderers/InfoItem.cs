namespace WhoAmI.CLI.Output.Renderers;

internal sealed record InfoItem(string Label, string? Value) {
    public bool IsBlank =>
        string.IsNullOrWhiteSpace(Label) &&
        string.IsNullOrWhiteSpace(Value);

    public string DisplayValue => Value ?? "-";

    public static InfoItem BlankLine() => new("", "");

    public static InfoItem Timestamp(string label, DateTimeOffset? date) => new(
        label,
        date?.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss zzz") ?? "Never"
    );

    public static InfoItem Identifier(string label, Guid guid) => new(
        label,
        guid.ToString()
    );
}
