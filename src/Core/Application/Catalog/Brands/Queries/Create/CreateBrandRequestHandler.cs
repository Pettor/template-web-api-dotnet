using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Brands.Queries.Create;

public class CreateBrandRequestHandler(IRepositoryWithEvents<Brand> repository)
    : IRequestHandler<CreateBrandRequest, Guid>
{
    public async Task<Guid> Handle(CreateBrandRequest request, CancellationToken cancellationToken)
    {
        var brand = new Brand(request.Name, request.Description);

        await repository.AddAsync(brand, cancellationToken);

        return brand.Id;
    }
}
