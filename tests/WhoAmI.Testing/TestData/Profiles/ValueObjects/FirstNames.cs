using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Testing.TestData.Profiles.ValueObjects;

public static class FirstNames {
    public const string Sergio = "Sérgio";

    public const string JohnChristopher = "John Christopher";

    public static FirstName CreateSergio() => new(Sergio);

    public static FirstName CreateJohnChristopher() => new(JohnChristopher);
}
