using WhoAmI.Application.Results;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles;

public static class ProfileErrors {
    public static Error NotFoundById(Guid id) =>
        new(
            "Profile.NotFound",
            "Profile was not found.",
            new Dictionary<string, object?> { ["ProfileId"] = id }
        );

    public static Error DuplicateEmail(Email email) =>
        new(
            "Profile.DuplicateEmail",
            "The email must be unique.",
            new Dictionary<string, object?> { ["ProfileEmail"] = email.Address }
        );
}
