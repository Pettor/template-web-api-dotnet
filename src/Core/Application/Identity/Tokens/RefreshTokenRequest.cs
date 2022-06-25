namespace MyHero.Application.Identity.Tokens;

public record RefreshTokenRequest(string Token, string RefreshToken);