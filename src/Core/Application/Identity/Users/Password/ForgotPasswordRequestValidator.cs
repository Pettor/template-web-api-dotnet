using WebApiTemplate.Application.Common.Validation;

namespace WebApiTemplate.Application.Identity.Users.Password;

public class ForgotPasswordRequestValidator : CustomValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator() =>
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid Email Address.");
}