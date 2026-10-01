using WhoAmI.Application.Results;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles;

public static class ProfileErrors {
    public static Error AlreadyDeleted(Guid id) =>
        new(
            "Profile.AlreadyDeleted",
            "The profile has already been deleted.",
            new Dictionary<string, object?> { ["ProfileId"] = id }
        );

    public static Error NotFoundById(Guid id) =>
        new(
            "Profile.NotFound",
            "Profile was not found.",
            new Dictionary<string, object?> { ["ProfileId"] = id }
        );

    public static Error NotFoundByEmail(string email) =>
        new(
            "Profile.NotFoundByEmail",
            "Profile was not found.",
            new Dictionary<string, object?> { ["ProfileEmail"] = email }
        );

    public static Error EmailAlreadyExists(Email email) =>
        new(
            "Profile.EmailAlreadyExists",
            "The email is already associated with a profile.",
            new Dictionary<string, object?> { ["ProfileEmail"] = email.Address }
        );
}
