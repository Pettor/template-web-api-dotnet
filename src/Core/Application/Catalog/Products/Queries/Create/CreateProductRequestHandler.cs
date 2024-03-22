using Backend.Application.Common.FileStorage;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;
using Backend.Domain.Common;
using Backend.Domain.Common.Events;

namespace Backend.Application.Catalog.Products.Queries.Create;

public class CreateProductRequestHandler(IRepository<Product> repository, IFileStorageService file) : IRequestHandler<CreateProductRequest, Guid>
{
    public async Task<Guid> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var productImagePath = await file.UploadAsync<Product>(request.Image, FileType.Image, cancellationToken);

        var product = new Product(request.Name, request.Description, request.Rate, request.BrandId, productImagePath);

        // Add Domain Events to be raised after the commit
        product.DomainEvents.Add(EntityCreatedEvent.WithEntity(product));

        await repository.AddAsync(product, cancellationToken);

        return product.Id;
    }
}
