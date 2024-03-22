using Backend.Application.Catalog.Products.Entities;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;
using Mapster;

namespace Backend.Application.Catalog.Products.Queries.Get;

public class GetProductViaDapperRequestHandler(IDapperRepository repository, IStringLocalizer<GetProductViaDapperRequestHandler> localizer)
    : IRequestHandler<GetProductViaDapperRequest, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductViaDapperRequest request, CancellationToken cancellationToken)
    {
        var product = await repository.QueryFirstOrDefaultAsync<Product>(
            $"SELECT * FROM public.\"Products\" WHERE \"Id\"  = '{request.Id}' AND \"Tenant\" = '@tenant'", cancellationToken: cancellationToken);

        _ = product ?? throw new NotFoundException(string.Format(localizer["product.notfound"], request.Id));

        return product.Adapt<ProductDto>();
    }
}
