namespace Backend.Application.Catalog.Products.Queries.Delete;

public class DeleteProductRequest(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}
