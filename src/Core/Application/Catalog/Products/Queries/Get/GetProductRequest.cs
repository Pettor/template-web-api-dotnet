using Backend.Application.Catalog.Products.Entities;

namespace Backend.Application.Catalog.Products.Queries.Get;

public class GetProductRequest : IRequest<ProductDetailsDto>
{
    public Guid Id { get; set; }

    public GetProductRequest(Guid id) => Id = id;
}
