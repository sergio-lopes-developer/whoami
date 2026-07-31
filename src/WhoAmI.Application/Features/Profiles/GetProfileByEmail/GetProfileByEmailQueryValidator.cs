using WhoAmI.Application.Abstractions.Validation;
using WhoAmI.Application.Results;
using WhoAmI.Application.Validation;
using WhoAmI.Domain.Profiles.ValueObjects;

namespace WhoAmI.Application.Features.Profiles.GetProfileByEmail;

internal sealed class GetProfileByEmailQueryValidator :
    IQueryValidator<GetProfileByEmailQuery>
{
    public IReadOnlyCollection<Error> Validate(GetProfileByEmailQuery query) {
        var errors = new List<Error>();

        Ensure.Field(query.Email, nameof(query.Email))
            .IsRequired()
            .Satisfies(Email.IsValid)
            .AddTo(errors);

        return errors;
    }
}
