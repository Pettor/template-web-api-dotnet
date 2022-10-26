﻿using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;
using Mapster;

namespace Backend.Application.Catalog.Products;

public class GetProductViaDapperRequestHandler : IRequestHandler<GetProductViaDapperRequest, ProductDto>
{
    private readonly IDapperRepository _repository;
    private readonly IStringLocalizer<GetProductViaDapperRequestHandler> _localizer;

    public GetProductViaDapperRequestHandler(IDapperRepository repository, IStringLocalizer<GetProductViaDapperRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<ProductDto> Handle(GetProductViaDapperRequest request, CancellationToken cancellationToken)
    {
        var product = await _repository.QueryFirstOrDefaultAsync<Product>(
            $"SELECT * FROM public.\"Products\" WHERE \"Id\"  = '{request.Id}' AND \"Tenant\" = '@tenant'", cancellationToken: cancellationToken);

        _ = product ?? throw new NotFoundException(string.Format(_localizer["product.notfound"], request.Id));

        return product.Adapt<ProductDto>();
    }
}