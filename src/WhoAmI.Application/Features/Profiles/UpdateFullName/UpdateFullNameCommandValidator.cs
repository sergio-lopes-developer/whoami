using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Results;
using WhoAmI.Application.Validation;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles.UpdateFullName;

internal sealed class UpdateFullNameCommandValidator :
    ICommandValidator<UpdateFullNameCommand>
{
    public IReadOnlyCollection<Error> Validate(UpdateFullNameCommand command) {
        var errors = new List<Error>();

        Ensure.Field(command.Id, nameof(command.Id))
            .IsRequired()
            .AddTo(errors);

        Ensure.Field(command.FirstName, nameof(command.FirstName))
            .IsRequired()
            .HasValidLength(FirstName.LengthConstraint)
            .AddTo(errors);

        Ensure.Field(command.LastName, nameof(command.LastName))
            .IsRequired()
            .HasValidLength(LastName.LengthConstraint)
            .AddTo(errors);

        return errors;
    }
}
