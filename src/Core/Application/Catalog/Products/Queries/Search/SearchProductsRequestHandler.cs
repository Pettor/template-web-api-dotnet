using Backend.Application.Catalog.Products.Entities;
using Backend.Application.Catalog.Products.Specifications;
using Backend.Application.Common.Models;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Products.Queries.Search;

public class SearchProductsRequestHandler(IReadRepository<Product> repository)
    : IRequestHandler<SearchProductsRequest, PaginationResponse<ProductDto>>
{
    public async Task<PaginationResponse<ProductDto>> Handle(SearchProductsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ProductsBySearchRequestWithBrandsSpec(request);
        return await repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
    }
}
