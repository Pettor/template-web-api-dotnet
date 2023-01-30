using System.Data.SqlClient;
using Backend.Application.Common.Persistence;
using Backend.Infrastructure.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Npgsql;

namespace Backend.Infrastructure.Persistence.ConnectionString;

internal class ConnectionStringValidator : IConnectionStringValidator
{
    private readonly DatabaseSettings _dbSettings;
    private readonly ILogger<ConnectionStringValidator> _logger;

    public ConnectionStringValidator(IOptions<DatabaseSettings> dbSettings, ILogger<ConnectionStringValidator> logger)
    {
        _dbSettings = dbSettings.Value;
        _logger = logger;
    }

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
            _logger.LogError($"Connection String Validation Exception : {ex.Message}");
            return false;
        }
    }
}
