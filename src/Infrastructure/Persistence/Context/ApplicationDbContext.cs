using Backend.Application.Common.Events;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Catalog;
using Backend.Infrastructure.Multitenancy;
using Backend.Infrastructure.Persistence.Configuration;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Persistence.Context;

public class ApplicationDbContext(
    IMultiTenantContextAccessor<TenantInfo> currentTenant,
    DbContextOptions options,
    ICurrentUser currentUser,
    ISerializerService serializer,
    IOptions<DatabaseSettings> dbSettings,
    IEventPublisher events
) : BaseDbContext(currentTenant, options, currentUser, serializer, dbSettings, events)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}
