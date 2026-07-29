using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Testing.TestData.Profiles.ValueObjects;

public static class Urls {
    public const string LinkedIn = "https://www.linkedin.com/in/username";

    public const string GitHub = "https://github.com/username";

    public const string Example = "https://example.com";

    public static Url CreateLinkedIn() => new(LinkedIn);

    public static Url CreateGitHub() => new(GitHub);

    public static Url CreateExample() => new(Example);
}
