using Emne7_Eksamen.Features.Members.Models;
using FluentValidation;

namespace Emne7_Eksamen.Validators.MemberValidators;

public class MemberValidator : AbstractValidator<Member>
{
    public MemberValidator()
    {
        RuleFor(m => m.Created)
            .NotEmpty().WithMessage("Created is required.");

        RuleFor(m => m.Updated)
            .NotEmpty().WithMessage("Updated is required.");

        RuleFor(m => m.HashedPassword)
            .NotEmpty().WithMessage("Hashed password is required.");
    }
}