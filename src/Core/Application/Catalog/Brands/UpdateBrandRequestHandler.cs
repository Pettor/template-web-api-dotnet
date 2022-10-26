﻿using WebApiTemplate.Application.Common.Exceptions;
using WebApiTemplate.Application.Common.Persistence;
using WebApiTemplate.Domain.Catalog;

namespace WebApiTemplate.Application.Catalog.Brands;

public class UpdateBrandRequestHandler : IRequestHandler<UpdateBrandRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Brand> _repository;
    private readonly IStringLocalizer<UpdateBrandRequestHandler> _localizer;

    public UpdateBrandRequestHandler(IRepositoryWithEvents<Brand> repository, IStringLocalizer<UpdateBrandRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(UpdateBrandRequest request, CancellationToken cancellationToken)
    {
        var brand = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = brand ?? throw new NotFoundException(string.Format(_localizer["brand.notfound"], request.Id));

        brand.Update(request.Name, request.Description);

        await _repository.UpdateAsync(brand, cancellationToken);

        return request.Id;
    }
}