using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Products;

public class ProductByIdWithBrandSpec : Specification<Product, ProductDetailsDto>, ISingleResultSpecification
{
    public ProductByIdWithBrandSpec(Guid id) =>
        Query
            .Where(p => p.Id == id)
            .Include(p => p.Brand);
}
