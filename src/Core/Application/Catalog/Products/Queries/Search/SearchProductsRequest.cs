using Backend.Application.Catalog.Products.Entities;
using Backend.Application.Common.Models;

namespace Backend.Application.Catalog.Products.Queries.Search;

public class SearchProductsRequest : PaginationFilter, IRequest<PaginationResponse<ProductDto>>
{
    public Guid? BrandId { get; set; }
    public decimal? MinimumRate { get; set; }
    public decimal? MaximumRate { get; set; }
}
