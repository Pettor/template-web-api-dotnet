using Backend.Application.Common.Validation;

namespace Backend.Application.Identity.Users.Password;

public class ResetPasswordValidator : CustomValidator<ResetPasswordRequest>
{
    public ResetPasswordValidator()
    {
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid Email Address.");
    }
}
