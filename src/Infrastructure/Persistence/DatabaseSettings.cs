namespace Backend.Infrastructure.Persistence;

public class DatabaseSettings
{
    public string DbProvider { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}
