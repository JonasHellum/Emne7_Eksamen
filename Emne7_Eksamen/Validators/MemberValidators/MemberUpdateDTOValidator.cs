using Emne7_Eksamen.Features.Members.Models;
using FluentValidation;

namespace Emne7_Eksamen.Validators.MemberValidators;

public class MemberUpdateDTOValidator : AbstractValidator<MemberUpdateDTO>
{
    public MemberUpdateDTOValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Length(2, 30)
            .WithMessage("First name must be between 2 and 30 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Length(2, 50)
            .WithMessage("Last name must be between 2 and 50 characters.");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .Must(gender => gender == 'M' || gender == 'F')
            .WithMessage("Gender must be either M or F");

        RuleFor(x => x.BirthYear)
            .NotEmpty().WithMessage("Birth year is required.")
            .Must(birthYear => birthYear >= 1000 &&  birthYear <=9999)
            .WithMessage("Birth year must be a 4 digit number.");

        // Optional but validated if provided
        RuleFor(x => x.Password)
            .Length(3, 30)
            .WithMessage("Password must be between 3 and 30 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Password));
    }
}