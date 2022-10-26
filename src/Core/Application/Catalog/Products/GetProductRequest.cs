namespace WebApiTemplate.Application.Catalog.Products;

public class GetProductRequest : IRequest<ProductDetailsDto>
{
    public Guid Id { get; set; }

    public GetProductRequest(Guid id) => Id = id;
}