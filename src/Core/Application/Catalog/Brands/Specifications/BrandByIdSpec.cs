using Backend.Application.Catalog.Brands.Entities;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands.Specifications;

public class BrandByIdSpec : Specification<Brand, BrandDto>, ISingleResultSpecification<Brand>
{
    public BrandByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id);
}
