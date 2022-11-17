using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Products;

public class ProductsByBrandSpec : Specification<Product>
{
    public ProductsByBrandSpec(Guid brandId) =>
        Query.Where(p => p.BrandId == brandId);
}
