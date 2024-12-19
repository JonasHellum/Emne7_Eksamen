using Emne7_Eksamen.Features.Races;
using FluentValidation;

namespace Emne7_Eksamen.Validators.RaceValidators;

public class RaceSearchParamsValidator : AbstractValidator<RaceSearchParams>
{
    public RaceSearchParamsValidator()
    {
        RuleFor(x => x.RaceId)
            .InclusiveBetween(1, 9999).WithMessage("MemberId must be between 1 and 9999.")
            .When(x => x.RaceId.HasValue);

        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 3000).WithMessage("Year must be between 2000 and 3000.")
            .When(x => x.Year.HasValue);
        
        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.")
            .When(x => x.Month.HasValue);
        
        RuleFor(x => x.Day)
            .InclusiveBetween(1, 31).WithMessage("Day must be between 1 and 31.")
            .When(x => x.Day.HasValue);
        
        RuleFor(x => x.Distance)
            .Must(distance => distance >= 1 && distance <= 32766)
            .WithMessage("Distance must be between 1 and 32766 meters.")
            .When(x => x.Distance.HasValue);
    }
}