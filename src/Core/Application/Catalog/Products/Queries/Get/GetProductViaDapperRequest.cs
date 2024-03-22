using Backend.Application.Catalog.Products.Entities;

namespace Backend.Application.Catalog.Products.Queries.Get;

public class GetProductViaDapperRequest : IRequest<ProductDto>
{
    public Guid Id { get; set; }

    public GetProductViaDapperRequest(Guid id) => Id = id;
}
