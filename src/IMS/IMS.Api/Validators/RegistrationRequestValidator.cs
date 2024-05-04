using FluentValidation;
using IMS.Api.RequestHandlers;

namespace IMS.Api.Validators;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequestHandler>
{
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit fahim")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character")
            .Must(x => x != null && x.Length > 8 && x.Length < 100).WithMessage("Password must be less than 100 characters long");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Username is required")
            .Length(2, 25)
            .Must(x => x.All(char.IsLetter)).WithMessage("UserName must contain only letters");
    }
}