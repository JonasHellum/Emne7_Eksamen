using Emne7_Eksamen.Features.Results;
using FluentValidation;

namespace Emne7_Eksamen.Validators.ResultValidators;

public class ResultValidator : AbstractValidator<Result>
{
    public ResultValidator()
    {
        RuleFor(x => x.RaceId)
            .NotEmpty().WithMessage("RaceId is required.");
        
        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("MemberId is required.");
    }
}