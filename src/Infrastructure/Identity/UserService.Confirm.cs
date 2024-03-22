using System.Globalization;
using System.Text;
using Backend.Application.Common.Exceptions;
using Backend.Infrastructure.Common;
using Backend.Shared.Multitenancy;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Identity;

internal partial class UserService
{
    private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
    {
        EnsureValidTenant();

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "api/users/confirm-email/";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
        verificationUri =
            QueryHelpers.AddQueryString(verificationUri, MultitenancyConstants.TenantIdName, currentTenant.Id!);
        return verificationUri;
    }

    public async Task<string> ConfirmEmailAsync(
        string userId,
        string code,
        string tenant,
        CancellationToken cancellationToken)
    {
        EnsureValidTenant();

        var user = await userManager.Users
            .Where(u => u.Id == userId && !u.EmailConfirmed)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new InternalServerException(localizer["An error occurred while confirming E-Mail."]);

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await userManager.ConfirmEmailAsync(user, code);

        return result.Succeeded
            ? string.Format(
                CultureInfo.CurrentCulture,
                localizer[
                    "Account Confirmed for E-Mail {0}. You can now use the /api/tokens endpoint to generate JWT."],
                user.Email)
            : throw new InternalServerException(string.Format(
                CultureInfo.CurrentCulture,
                localizer["An error occurred while confirming {0}"],
                user.Email));
    }

    public async Task<string> ConfirmPhoneNumberAsync(string userId, string code)
    {
        EnsureValidTenant();

        var user = await userManager.FindByIdAsync(userId);

        _ = user ?? throw new InternalServerException(localizer["An error occurred while confirming Mobile Phone."]);

        var result = await userManager.ChangePhoneNumberAsync(user, user.PhoneNumber ?? "N/A", code);

        return result.Succeeded
            ? user.EmailConfirmed
                ? string.Format(
                    CultureInfo.CurrentCulture,
                    localizer[
                        "Account Confirmed for Phone Number {0}. You can now use the /api/tokens endpoint to generate JWT."],
                    user.PhoneNumber)
                : string.Format(
                    CultureInfo.CurrentCulture,
                    localizer[
                        "Account Confirmed for Phone Number {0}. You should confirm your E-mail before using the /api/tokens endpoint to generate JWT."],
                    user.PhoneNumber)
            : throw new InternalServerException(string.Format(
                CultureInfo.CurrentCulture,
                localizer["An error occurred while confirming {0}"],
                user.PhoneNumber));
    }
}
