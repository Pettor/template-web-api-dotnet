using Backend.Application.Catalog.Products.Entities;
using Backend.Application.Catalog.Products.Specifications;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Products.Queries.Get;

public class GetProductRequestHandler(IRepository<Product> repository, IStringLocalizer<GetProductRequestHandler> localizer)
    : IRequestHandler<GetProductRequest, ProductDetailsDto>
{
    public async Task<ProductDetailsDto> Handle(GetProductRequest request, CancellationToken cancellationToken) =>
        await repository.FirstOrDefaultAsync(
            new ProductByIdWithBrandSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(localizer["product.notfound"], request.Id));
}
