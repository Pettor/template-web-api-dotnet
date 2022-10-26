using Backend.Infrastructure.Persistence.Configuration;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Multitenancy;

public class TenantDbContext : EFCoreStoreDbContext<FshTenantInfo>
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FshTenantInfo>().ToTable("Tenants", SchemaNames.MultiTenancy);
    }
}