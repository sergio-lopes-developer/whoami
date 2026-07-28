namespace WhoAmI.Testing.Helpers;

public static class TextInputs {
    public static IEnumerable<object[]> Blank => [
        [""],
        [" "],
        ["\t"],
        ["\n"]
    ];

    public static string CreateStringWithLength(int length) {
        ArgumentOutOfRangeException.ThrowIfNegative(length);
        return new string('a', length);
    }
}
