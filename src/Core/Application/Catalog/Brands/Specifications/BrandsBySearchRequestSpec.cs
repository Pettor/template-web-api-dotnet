using Backend.Application.Catalog.Brands.Entities;
using Backend.Application.Catalog.Brands.Queries.Search;
using Backend.Application.Common.Models;
using Backend.Application.Common.Specification;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands.Specifications;

public class BrandsBySearchRequestSpec : EntitiesByPaginationFilterSpec<Brand, BrandDto>
{
    public BrandsBySearchRequestSpec(SearchBrandsRequest request)
        : base(request) => Query.OrderBy(c => c.Name, !request.HasOrderBy());
}
