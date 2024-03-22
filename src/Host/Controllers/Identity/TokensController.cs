using Backend.Application.Common.Exceptions;
using Backend.Application.Identity.Tokens;
using Backend.Infrastructure.OpenApi;
using Org.BouncyCastle.Ocsp;

namespace Backend.Host.Controllers.Identity;

public sealed class TokensController(ITokenService tokenService) : VersionNeutralApiController
{
    [HttpPost]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request an access token using credentials.", "")]
    public async Task<ActionResult<TokenResponse>> GetTokenAsync(
        TokenRequest request,
        CancellationToken cancellationToken)
    {
        var tokenResult = await tokenService.GetTokenAsync(request, GetIpAddress(), cancellationToken);

        AddRefreshTokenCookie(tokenResult.RefreshToken);
        return new TokenResponse(tokenResult.Token, tokenResult.RefreshTokenExpiryTime);
    }

    [HttpDelete]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request an access token using credentials.", "")]
    public ActionResult<TokenResponse> RemoveTokenAsync()
    {
        Response.Cookies.Delete("refresh_token", CreateCookeOptions());
        return Ok();
    }

    [HttpGet("refresh")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request an access token using a refresh token.", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Search))]
    public async Task<ActionResult<TokenResponse>> RefreshAsync()
    {
        if (!Request.Cookies.TryGetValue("refresh_token", out var accessToken))
        {
            return UnprocessableEntity();
        }

        try
        {
            var tokenResult = await tokenService.RefreshTokenAsync(accessToken, GetIpAddress());
            AddRefreshTokenCookie(tokenResult.RefreshToken);
            return new TokenResponse(tokenResult.Token, tokenResult.RefreshTokenExpiryTime);
        }
        catch (UnauthorizedException)
        {
            Response.Cookies.Delete("refresh_token", CreateCookeOptions());
            throw;
        }
    }

    private void AddRefreshTokenCookie(string refreshToken)
    {
        // Apply RefreshToken to secure cookie
        Response.Cookies.Append("refresh_token", refreshToken, CreateCookeOptions());
    }

    private static CookieOptions CreateCookeOptions()
    {
        return new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict, Secure = true };
    }

    private string GetIpAddress()
    {
        const string Na = "N/A";

        var headers = Request.Headers;
        if (headers.TryGetValue("X-Forwarded-For", out var forwardedForHeader))
        {
            return forwardedForHeader.ToString();
        }

        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? Na;
    }
}
