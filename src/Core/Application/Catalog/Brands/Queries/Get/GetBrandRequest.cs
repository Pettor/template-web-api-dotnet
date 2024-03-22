using Backend.Application.Catalog.Brands.Entities;

namespace Backend.Application.Catalog.Brands.Queries.Get;

public class GetBrandRequest : IRequest<BrandDto>
{
    public Guid Id { get; set; }

    public GetBrandRequest(Guid id) => Id = id;
}
