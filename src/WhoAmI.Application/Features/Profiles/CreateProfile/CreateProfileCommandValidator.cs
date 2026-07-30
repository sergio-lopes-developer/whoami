using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Results;
using WhoAmI.Application.Validation;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles.CreateProfile;

internal sealed class CreateProfileCommandValidator :
    ICommandValidator<CreateProfileCommand>
{
    public IReadOnlyCollection<Error> Validate(CreateProfileCommand command) {
        var errors = new List<Error>();

        Ensure.Field(command.FirstName, nameof(command.FirstName))
            .IsRequired()
            .HasValidLength(FirstName.LengthConstraint)
            .AddTo(errors);

        Ensure.Field(command.LastName, nameof(command.LastName))
            .IsRequired()
            .HasValidLength(LastName.LengthConstraint)
            .AddTo(errors);

        Ensure.Field(command.Email, nameof(command.Email))
            .IsRequired()
            .Satisfies(Email.IsValid)
            .AddTo(errors);

        Ensure.Field(command.GitHub, nameof(command.GitHub))
            .IsRequired()
            .Satisfies(Url.IsValid)
            .AddTo(errors);

        Ensure.Field(command.LinkedIn, nameof(command.LinkedIn))
            .IsRequired()
            .Satisfies(Url.IsValid)
            .AddTo(errors);

        return errors;
    }
}
