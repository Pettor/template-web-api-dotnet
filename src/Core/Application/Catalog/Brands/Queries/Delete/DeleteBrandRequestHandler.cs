using Backend.Application.Catalog.Products;
using Backend.Application.Catalog.Products.Specifications;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands.Queries.Delete;

public class DeleteBrandRequestHandler(
    IRepositoryWithEvents<Brand> brandRepo,
    IReadRepository<Product> productRepo,
    IStringLocalizer<DeleteBrandRequestHandler> localizer
) : IRequestHandler<DeleteBrandRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents

    public async Task<Guid> Handle(DeleteBrandRequest request, CancellationToken cancellationToken)
    {
        if (await productRepo.AnyAsync(new ProductsByBrandSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(localizer["brand.cannotbedeleted"]);
        }

        var brand = await brandRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = brand ?? throw new NotFoundException(localizer["brand.notfound"]);

        await brandRepo.DeleteAsync(brand, cancellationToken);

        return request.Id;
    }
}
