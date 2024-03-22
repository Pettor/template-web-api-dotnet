using Backend.Application.Catalog.Brands.Entities;
using Backend.Application.Common.Models;

namespace Backend.Application.Catalog.Brands.Queries.Search;

public class SearchBrandsRequest : PaginationFilter, IRequest<PaginationResponse<BrandDto>>
{
}
