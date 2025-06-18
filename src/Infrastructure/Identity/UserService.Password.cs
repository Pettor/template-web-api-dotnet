using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Mailing;
using Backend.Application.Identity.Users.Password;
using Microsoft.AspNetCore.WebUtilities;

namespace Backend.Infrastructure.Identity;

internal partial class UserService
{
    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        EnsureValidTenant();

        var user = await userManager.FindByEmailAsync(request.Email.Normalize());
        if (user is null || !await userManager.IsEmailConfirmedAsync(user))
        {
            // Don't reveal that the user does not exist or is not confirmed
            throw new InternalServerException(localizer["An Error has occurred!"]);
        }

        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        const string route = "account/reset-password";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
        var mailRequest = new MailRequest(
            new List<string> { request.Email },
            localizer["Reset Password"],
            localizer[
                $"Your Password Reset Token is '{code}'. You can reset your password using the {endpointUri} Endpoint."
            ]
        );
        jobService.Enqueue(() => mailService.SendAsync(mailRequest));

        return localizer["Password Reset Mail has been sent to your authorized Email."];
    }

    public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email?.Normalize());

        // Don't reveal that the user does not exist
        _ = user ?? throw new InternalServerException(localizer["An Error has occurred!"]);

        var result = await userManager.ResetPasswordAsync(user, request.Token, request.Password);

        return result.Succeeded
            ? localizer["Password Reset Successful!"]
            : throw new InternalServerException(localizer["An Error has occurred!"]);
    }

    public async Task ChangePasswordAsync(ChangePasswordRequest model, string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        _ = user ?? throw new NotFoundException(localizer["User Not Found."]);

        var result = await userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

        if (!result.Succeeded)
        {
            throw new InternalServerException(
                localizer["Change password failed"],
                result.GetErrors(localizer)
            );
        }
    }
}
