using System.Data.SqlClient;
using Backend.Application.Common.Persistence;
using Backend.Infrastructure.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Npgsql;

namespace Backend.Infrastructure.Persistence.ConnectionString;

internal class ConnectionStringValidator(IOptions<DatabaseSettings> dbSettings, ILogger<ConnectionStringValidator> logger)
    : IConnectionStringValidator
{
    private readonly DatabaseSettings _dbSettings = dbSettings.Value;

    public bool TryValidate(string connectionString, string? dbProvider = null)
    {
        if (string.IsNullOrWhiteSpace(dbProvider))
        {
            dbProvider = _dbSettings.DbProvider;
        }

        try
        {
            switch (dbProvider?.ToUpperInvariant())
            {
                case DbProviderKeys.Npgsql:
                    var postgresqlcs = new NpgsqlConnectionStringBuilder(connectionString);
                    break;

            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError($"Connection String Validation Exception : {ex.Message}");
            return false;
        }
    }
}
