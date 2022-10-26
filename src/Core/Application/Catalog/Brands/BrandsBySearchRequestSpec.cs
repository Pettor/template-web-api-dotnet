using WebApiTemplate.Application.Common.Models;
using WebApiTemplate.Application.Common.Specification;
using WebApiTemplate.Domain.Catalog;

namespace WebApiTemplate.Application.Catalog.Brands;

public class BrandsBySearchRequestSpec : EntitiesByPaginationFilterSpec<Brand, BrandDto>
{
    public BrandsBySearchRequestSpec(SearchBrandsRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Name, !request.HasOrderBy());
}