using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiTemplate.Domain.Catalog;

namespace WebApiTemplate.Infrastructure.Persistence.Configuration;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.IsMultiTenant();

        builder
            .Property(b => b.Name)
            .HasMaxLength(1024);

        builder
            .Property(p => p.ImagePath)
            .HasMaxLength(2048);
    }
}