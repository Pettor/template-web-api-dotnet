using Backend.Application.Common.Persistence;
using Backend.Infrastructure.Common;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Backend.Infrastructure.Persistence.ConnectionString;

public class ConnectionStringSecurer(IOptions<DatabaseSettings> dbSettings)
    : IConnectionStringSecurer
{
    private const string HiddenValueDefault = "*******";
    private readonly DatabaseSettings _dbSettings = dbSettings.Value;

    public string? MakeSecure(string? connectionString, string? dbProvider)
    {
        if (connectionString is null || string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }

        if (string.IsNullOrWhiteSpace(dbProvider))
        {
            dbProvider = _dbSettings.DbProvider;
        }

        return dbProvider?.ToLower() switch
        {
            DbProviderKeys.Npgsql => MakeSecureNpgsqlConnectionString(connectionString),
            _ => connectionString,
        };
    }

    private static string MakeSecureNpgsqlConnectionString(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        if (!string.IsNullOrEmpty(builder.Password))
        {
            builder.Password = HiddenValueDefault;
        }

        if (!string.IsNullOrEmpty(builder.Username))
        {
            builder.Username = HiddenValueDefault;
        }

        return builder.ToString();
    }
}
