using Emne7_Eksamen.Features.Races;
using FluentValidation;

namespace Emne7_Eksamen.Validators.RaceValidators;

public class RaceValidator : AbstractValidator<Race>
{
    public RaceValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("MemberId is required.");
    }
}