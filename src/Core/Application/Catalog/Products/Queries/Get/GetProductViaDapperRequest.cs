using Backend.Application.Catalog.Products.Entities;

namespace Backend.Application.Catalog.Products.Queries.Get;

public class GetProductViaDapperRequest(Guid id) : IRequest<ProductDto>
{
    public Guid Id { get; set; } = id;
}
