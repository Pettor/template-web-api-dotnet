namespace Backend.Infrastructure.BackgroundJobs;

public class HangfireStorageSettings
{
    public string StorageProvider { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}
