using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands;

public class BrandByIdSpec : Specification<Brand, BrandDto>, ISingleResultSpecification
{
    public BrandByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id);
}
