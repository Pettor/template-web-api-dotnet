namespace Backend.Application.Identity.Tokens;

public record TokenResult(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
