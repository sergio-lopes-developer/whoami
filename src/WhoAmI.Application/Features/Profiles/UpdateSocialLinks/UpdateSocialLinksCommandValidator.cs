using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Results;
using WhoAmI.Application.Validation;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles.UpdateSocialLinks;

internal sealed class UpdateSocialLinksCommandValidator :
    ICommandValidator<UpdateSocialLinksCommand>
{
    public IReadOnlyCollection<Error> Validate(
        UpdateSocialLinksCommand command
    ) {
        var errors = new List<Error>();

        Ensure.Field(command.Id, nameof(command.Id))
            .IsRequired()
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
