using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Testing.TestData.Profiles.ValueObjects;

public static class FullNames {
    public static FullName CreateSergioLopes() => new(
        FirstNames.CreateSergio(),
        LastNames.CreateLopes()
    );

    public static FullName CreateJohnChristopherLewisMiller() => new(
        FirstNames.CreateJohnChristopher(),
        LastNames.CreateLewisMiller()
    );
}
