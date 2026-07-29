using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Testing.TestData.Profiles.ValueObjects;

public static class LastNames {
    public const string Lopes = "Lopes";

    public const string LewisMiller = "Lewis Miller";

    public static LastName CreateLopes() => new(Lopes);

    public static LastName CreateLewisMiller() => new(LewisMiller);
}
