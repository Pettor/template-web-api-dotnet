using Backend.Application.Catalog.Brands.Entities;
using Backend.Application.Catalog.Brands.Specifications;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands.Queries.Get;

public class GetBrandRequestHandler(IRepository<Brand> repository, IStringLocalizer<GetBrandRequestHandler> localizer)
    : IRequestHandler<GetBrandRequest, BrandDto>
{
    public async Task<BrandDto> Handle(GetBrandRequest request, CancellationToken cancellationToken) =>
        await repository.FirstOrDefaultAsync(
            new BrandByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(localizer["brand.notfound"], request.Id));
}
