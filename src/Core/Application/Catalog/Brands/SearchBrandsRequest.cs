using WebApiTemplate.Application.Common.Models;

namespace WebApiTemplate.Application.Catalog.Brands;

public class SearchBrandsRequest : PaginationFilter, IRequest<PaginationResponse<BrandDto>>
{
}