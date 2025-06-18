using Backend.Application.Common.Exceptions;
using Backend.Application.Common.FileStorage;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;
using Backend.Domain.Common;
using Backend.Domain.Common.Events;

namespace Backend.Application.Catalog.Products.Queries.Update;

public class UpdateProductRequestHandler(
    IRepository<Product> repository,
    IStringLocalizer<UpdateProductRequestHandler> localizer,
    IFileStorageService file
) : IRequestHandler<UpdateProductRequest, Guid>
{
    public async Task<Guid> Handle(
        UpdateProductRequest request,
        CancellationToken cancellationToken
    )
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);

        _ =
            product
            ?? throw new NotFoundException(
                string.Format(localizer["product.notfound"], request.Id)
            );

        // Remove old image if flag is set
        if (request.DeleteCurrentImage)
        {
            var currentProductImagePath = product.ImagePath;
            if (!string.IsNullOrEmpty(currentProductImagePath))
            {
                var root = Directory.GetCurrentDirectory();
                file.Remove(Path.Combine(root, currentProductImagePath));
            }

            product = product.ClearImagePath();
        }

        var productImagePath = request.Image is not null
            ? await file.UploadAsync<Product>(request.Image, FileType.Image, cancellationToken)
            : null;

        var updatedProduct = product.Update(
            request.Name,
            request.Description,
            request.Rate,
            request.BrandId,
            productImagePath
        );

        // Add Domain Events to be raised after the commit
        product.DomainEvents.Add(EntityUpdatedEvent.WithEntity(product));

        await repository.UpdateAsync(updatedProduct, cancellationToken);

        return request.Id;
    }
}
