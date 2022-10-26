﻿using WebApiTemplate.Application.Common.Exceptions;
using WebApiTemplate.Application.Common.Persistence;
using WebApiTemplate.Domain.Catalog;
using WebApiTemplate.Domain.Common.Events;

namespace WebApiTemplate.Application.Catalog.Products;

public class DeleteProductRequestHandler : IRequestHandler<DeleteProductRequest, Guid>
{
    private readonly IRepository<Product> _repository;
    private readonly IStringLocalizer<DeleteProductRequestHandler> _localizer;

    public DeleteProductRequestHandler(IRepository<Product> repository, IStringLocalizer<DeleteProductRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<Guid> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = product ?? throw new NotFoundException(_localizer["product.notfound"]);

        // Add Domain Events to be raised after the commit
        product.DomainEvents.Add(EntityDeletedEvent.WithEntity(product));

        await _repository.DeleteAsync(product, cancellationToken);

        return request.Id;
    }
}