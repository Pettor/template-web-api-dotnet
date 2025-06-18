using Serilog;

namespace Backend.Infrastructure.Auth.AzureAd;

internal static class AzureAdJwtBearerEventsLoggingExtensions
{
    public static void AuthenticationFailed(this ILogger logger, Exception e) =>
        logger.Error("Authentication failed Exception: {e}", e);

    public static void TokenReceived(this ILogger logger) =>
        logger.Debug("Received a bearer token");

    public static void TokenValidationStarted(
        this ILogger logger,
        string? userId,
        string? issuer
    ) =>
        logger.Debug(
            "Token Validation Started for User: {userId} Issuer: {issuer}",
            userId,
            issuer
        );

    public static void TokenValidationFailed(this ILogger logger, string? userId, string? issuer) =>
        logger.Warning("Tenant is not registered User: {userId} Issuer: {issuer}", userId, issuer);

    public static void TokenValidationSucceeded(
        this ILogger logger,
        string userId,
        string issuer
    ) =>
        logger.Debug("Token validation succeeded: User: {userId} Issuer: {issuer}", userId, issuer);
}
