using Emne7_Eksamen.Features.Races;
using FluentValidation;

namespace Emne7_Eksamen.Validators.RaceValidators;

public class RaceRegistrationDTOValidator : AbstractValidator<RaceRegistrationDTO>
{
    public RaceRegistrationDTOValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.");
        
        RuleFor(x => x.Distance)
            .NotEmpty().WithMessage("Distance is required.")
            .Must(distance => distance >= 1 && distance <= 32766)
            .WithMessage("Distance must be between 1 and 32766 meters.");
    }
}