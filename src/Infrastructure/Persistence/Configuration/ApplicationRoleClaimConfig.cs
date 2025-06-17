using Backend.Infrastructure.Identity;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Persistence.Configuration;

public class ApplicationRoleClaimConfig : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder) =>
        builder.ToTable("RoleClaims", SchemaNames.Identity).IsMultiTenant();
}
