using Emne7_Eksamen.Features.Races;
using FluentValidation;
using FluentValidation.Validators;

namespace Emne7_Eksamen.Validators.RaceValidators;

public class RaceUpdateDTOValidator : AbstractValidator<RaceUpdateDTO>
{
    public RaceUpdateDTOValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.");
        
        RuleFor(x => x.Distance)
            .NotEmpty().WithMessage("Distance is required.")
            .Must(distance => distance >= 1 && distance <= 32766)
            .WithMessage("Distance must be between 1 and 32766 meters.");
    }
}