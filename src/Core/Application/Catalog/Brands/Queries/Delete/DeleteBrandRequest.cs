namespace Backend.Application.Catalog.Brands.Queries.Delete;

public class DeleteBrandRequest(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}
