using FluentValidation;
using Social.Domain.Aggregates.UserProfileAggregates;

namespace Social.Domain.Validators.UserProfileValidators;

internal class BasicInfoValidator : AbstractValidator<BasicInfo>
{
    public BasicInfoValidator()
    {
        RuleFor(info => info.FirstName)
            .NotNull().WithMessage("First Name is required. It is currently null")
            .MinimumLength(3).WithMessage("First name must be at least 3 characters long")
            .MaximumLength(50).WithMessage("Last name must be at most 50 characters long");
        
        RuleFor(info => info.LastName)
            .NotNull().WithMessage("Last Name is required. It is currently null")
            .MinimumLength(3).WithMessage("Last name must be at least 3 characters long")
            .MaximumLength(50).WithMessage("Last name must be at most 50 characters long");

        RuleFor(info => info.EmailAddress)
            .NotNull().WithMessage("E-mail address is required")
            .EmailAddress().WithMessage("Provided string is not an email");

        RuleFor(info => info.DateOfBirth)
            .InclusiveBetween(new DateTime(DateTime.Now.AddYears(-100).Ticks),
                new DateTime(DateTime.Now.AddYears(-18).Ticks))
            .WithMessage("Age has to be between 18 and 100");
        
            
        
    }
}