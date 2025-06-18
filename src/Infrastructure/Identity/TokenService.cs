using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Application.Common.Exceptions;
using Backend.Application.Identity.Tokens;
using Backend.Infrastructure.Auth;
using Backend.Infrastructure.Auth.Jwt;
using Backend.Infrastructure.Multitenancy;
using Backend.Shared.Authorization;
using Backend.Shared.Multitenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Infrastructure.Identity;

internal class TokenService(
    UserManager<ApplicationUser> userManager,
    IOptions<JwtSettings> jwtSettings,
    IStringLocalizer<TokenService> localizer,
    TenantInfo? currentTenant,
    IOptions<SecuritySettings> securitySettings
) : ITokenService
{
    private readonly SecuritySettings _securitySettings = securitySettings.Value;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<TokenResult> GetTokenAsync(
        TokenRequest request,
        string ipAddress,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(currentTenant?.Id))
        {
            throw new UnauthorizedException(localizer["tenant.invalid"]);
        }

        var user = await userManager.FindByEmailAsync(request.Email.Trim().Normalize());
        if (user is null)
        {
            throw new UnauthorizedException(localizer["auth.failed"]);
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException(localizer["identity.usernotactive"]);
        }

        if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
        {
            throw new UnauthorizedException(localizer["identity.emailnotconfirmed"]);
        }

        if (currentTenant.Id != MultitenancyConstants.Root.Id)
        {
            if (!currentTenant.IsActive)
            {
                throw new UnauthorizedException(localizer["tenant.inactive"]);
            }

            if (DateTime.UtcNow > currentTenant.ValidUpto)
            {
                throw new UnauthorizedException(localizer["tenant.expired"]);
            }
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedException(localizer["identity.invalidcredentials"]);
        }

        return await GenerateTokensAndUpdateUser(user, ipAddress, request.RememberMe);
    }

    public async Task<TokenResult> RefreshTokenAsync(string accessToken, string ipAddress)
    {
        var user = userManager.Users.FirstOrDefault(u => u.RefreshToken == accessToken);
        if (user is null)
        {
            throw new UnauthorizedException(localizer["auth.failed"]);
        }

        if (user.RefreshToken != accessToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException(localizer["identity.invalidrefreshtoken"]);
        }

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    private async Task<TokenResult> GenerateTokensAndUpdateUser(
        ApplicationUser user,
        string ipAddress,
        bool rememberMe = true
    )
    {
        var token = GenerateJwt(user, ipAddress);

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = rememberMe
            ? DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)
            : DateTime.UtcNow;

        await userManager.UpdateAsync(user);

        return new TokenResult(token, user.RefreshToken, user.RefreshTokenExpiryTime);
    }

    private string GenerateJwt(ApplicationUser user, string ipAddress) =>
        GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

    private IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress) =>
        new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ApiClaims.Fullname, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Name, user.FirstName ?? string.Empty),
            new(ClaimTypes.Surname, user.LastName ?? string.Empty),
            new(ApiClaims.IpAddress, ipAddress),
            new(ApiClaims.Tenant, currentTenant!.Id),
            new(ApiClaims.ImageUrl, user.ImageUrl ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
        };

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateEncryptedToken(
        SigningCredentials signingCredentials,
        IEnumerable<Claim> claims
    )
    {
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            signingCredentials: signingCredentials
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials GetSigningCredentials()
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        }

        var secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(
            new SymmetricSecurityKey(secret),
            SecurityAlgorithms.HmacSha256
        );
    }
}
