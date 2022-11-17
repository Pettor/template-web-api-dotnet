using Backend.Application.Common.Models;

namespace Backend.Application.Catalog.Brands;

public class SearchBrandsRequest : PaginationFilter, IRequest<PaginationResponse<BrandDto>>
{
}
