using System.Reflection;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Catalog;
using Backend.Infrastructure.Persistence.Context;
using Backend.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;

namespace Backend.Infrastructure.Catalog;

public class BrandSeeder(ISerializerService serializerService, ILogger<BrandSeeder> logger, ApplicationDbContext db)
    : ICustomSeeder
{
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!db.Brands.Any())
        {
            logger.LogInformation("Started to Seed Brands.");

            // Here you can use your own logic to populate the database.
            // As an example, I am using a JSON file to populate the database.
            var brandData = await File.ReadAllTextAsync(path + "/Catalog/brands.json", cancellationToken);
            var brands = serializerService.Deserialize<List<Brand>>(brandData);

            if (brands != null)
            {
                foreach (var brand in brands)
                {
                    await db.Brands.AddAsync(brand, cancellationToken);
                }
            }

            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seeded Brands.");
        }
    }
}
