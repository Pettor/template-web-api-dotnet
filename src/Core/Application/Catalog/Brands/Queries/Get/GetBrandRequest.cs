using Backend.Application.Catalog.Brands.Entities;

namespace Backend.Application.Catalog.Brands.Queries.Get;

public class GetBrandRequest(Guid id) : IRequest<BrandDto>
{
    public Guid Id { get; set; } = id;
}
