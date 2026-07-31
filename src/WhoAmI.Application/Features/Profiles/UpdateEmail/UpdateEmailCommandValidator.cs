using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Results;
using WhoAmI.Application.Validation;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles.UpdateEmail;

internal sealed class UpdateEmailCommandValidator :
    ICommandValidator<UpdateEmailCommand>
{
    public IReadOnlyCollection<Error> Validate(UpdateEmailCommand command) {
        var errors = new List<Error>();

        Ensure.Field(command.Id, nameof(command.Id))
            .IsRequired()
            .AddTo(errors);

        Ensure.Field(command.Email, nameof(command.Email))
            .IsRequired()
            .Satisfies(Email.IsValid)
            .AddTo(errors);

        return errors;
    }
}
