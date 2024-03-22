using Backend.Application.Catalog.Brands.Entities;
using Backend.Application.Catalog.Brands.Specifications;
using Backend.Application.Common.Models;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands.Queries.Search;

public class SearchBrandsRequestHandler : IRequestHandler<SearchBrandsRequest, PaginationResponse<BrandDto>>
{
    private readonly IReadRepository<Brand> _repository;

    public SearchBrandsRequestHandler(IReadRepository<Brand> repository) => _repository = repository;

    public async Task<PaginationResponse<BrandDto>> Handle(SearchBrandsRequest request, CancellationToken cancellationToken)
    {
        var spec = new BrandsBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}
