using System.Runtime.InteropServices.JavaScript;
using Emne7_Eksamen.Features.Members.Models;
using FluentValidation;

namespace Emne7_Eksamen.Validators.MemberValidators;

public class MemberSearchParamsValidator : AbstractValidator<MemberSearchParams>
{
    public MemberSearchParamsValidator()
    {
        RuleFor(m => m.MemberId)
            .InclusiveBetween(1, 9999).WithMessage("MemberId must be between 1 and 9999.")
            .When(m => m.MemberId.HasValue);
        
        RuleFor(m => m.FirstName)
            .Length(1, 30).WithMessage("First name must be between 1 and 30 characters.")
            .When(m => m.FirstName !=null);
        
        RuleFor(m => m.LastName)
            .Length(1, 50).WithMessage("Last name must be between 1 and 50 characters.")
            .When(m => m.LastName !=null);
    }
}