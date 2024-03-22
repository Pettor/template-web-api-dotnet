using Backend.Application.Catalog.Products.Entities;

namespace Backend.Application.Catalog.Products.Queries.Get;

public class GetProductRequest(Guid id) : IRequest<ProductDetailsDto>
{
    public Guid Id { get; set; } = id;
}
