using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Testing.TestData.Profiles.ValueObjects;

public static class Emails {
    public const string Valid = "email.test@gmail.com";

    public const string Sergio = "sergio.testmail@mailprovider.com";

    public static Email CreateValid() => new(Valid);

    public static Email CreateSergio() => new(Sergio);
}
