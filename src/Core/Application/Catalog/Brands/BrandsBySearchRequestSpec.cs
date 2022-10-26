using Backend.Application.Common.Models;
using Backend.Application.Common.Specification;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands;

public class BrandsBySearchRequestSpec : EntitiesByPaginationFilterSpec<Brand, BrandDto>
{
    public BrandsBySearchRequestSpec(SearchBrandsRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Name, !request.HasOrderBy());
}