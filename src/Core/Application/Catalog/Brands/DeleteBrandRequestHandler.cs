using Backend.Application.Catalog.Products;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands;

public class DeleteBrandRequestHandler : IRequestHandler<DeleteBrandRequest, Guid>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Brand> _brandRepo;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IStringLocalizer<DeleteBrandRequestHandler> _localizer;

    public DeleteBrandRequestHandler(IRepositoryWithEvents<Brand> brandRepo, IReadRepository<Product> productRepo, IStringLocalizer<DeleteBrandRequestHandler> localizer) =>
        (_brandRepo, _productRepo, _localizer) = (brandRepo, productRepo, localizer);

    public async Task<Guid> Handle(DeleteBrandRequest request, CancellationToken cancellationToken)
    {
        if (await _productRepo.AnyAsync(new ProductsByBrandSpec(request.Id), cancellationToken))
        {
            throw new ConflictException(_localizer["brand.cannotbedeleted"]);
        }

        var brand = await _brandRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = brand ?? throw new NotFoundException(_localizer["brand.notfound"]);

        await _brandRepo.DeleteAsync(brand, cancellationToken);

        return request.Id;
    }
}
