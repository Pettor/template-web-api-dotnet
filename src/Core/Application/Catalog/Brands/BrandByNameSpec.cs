using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands;

public class BrandByNameSpec : Specification<Brand>, ISingleResultSpecification
{
    public BrandByNameSpec(string name) =>
        Query.Where(b => b.Name == name);
}