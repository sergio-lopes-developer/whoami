using WhoAmI.Domain.Profiles.Events;
using WhoAmI.Domain.Profiles.ValueObjects;
using WhoAmI.Domain.Shared.Base;
using WhoAmI.Domain.Shared.Guards;

namespace WhoAmI.Domain.Profiles;

public sealed class Profile : AggregateRoot {
    public FullName FullName { get; private set; } = null!;

    public Email Email { get; private set; } = null!;

    public Url LinkedIn { get; private set; } = null!;

    public Url GitHub { get; private set; } = null!;

    internal Profile(
        Guid id,
        FullName fullName,
        Email email,
        Url linkedIn,
        Url gitHub
    ) : base(id) {
        Guard.AgainstNull(fullName, nameof(fullName));
        Guard.AgainstNull(email, nameof(email));
        Guard.AgainstNull(linkedIn, nameof(linkedIn));
        Guard.AgainstNull(gitHub, nameof(gitHub));

        FullName = fullName;
        Email = email;
        LinkedIn = linkedIn;
        GitHub = gitHub;
    }

    // ReSharper disable once UnusedMember.Local
    private Profile() { } // required by EF

    public static Profile Create(
        FullName fullName,
        Email email,
        Url linkedIn,
        Url gitHub
    ) {
        var profile = new Profile(
            Guid.NewGuid(),
            fullName,
            email,
            linkedIn,
            gitHub
        );

        profile.AddDomainEvent(new ProfileCreatedEvent(profile.Id));

        return profile;
    }

    public void UpdateFullName(FullName fullName) {
        Guard.AgainstNull(fullName, nameof(fullName));
        FullName = fullName;
    }

    public void UpdateEmail(Email email) {
        Guard.AgainstNull(email, nameof(email));

        if (Email == email) return;

        Email = email;

        AddDomainEvent(new EmailUpdatedEvent(Id, email.Address));
    }

    public void UpdateSocialLinks(Url linkedIn, Url gitHub) {
        Guard.AgainstNull(linkedIn, nameof(linkedIn));
        Guard.AgainstNull(gitHub, nameof(gitHub));

        LinkedIn = linkedIn;
        GitHub = gitHub;
    }

    public override string ToString() => $"Profile - {FullName}";
}
