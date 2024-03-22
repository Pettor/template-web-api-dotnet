namespace Backend.Application.Catalog.Brands.Queries.Delete;

public class DeleteBrandRequest : IRequest<Guid>
{
    public Guid Id { get; set; }

    public DeleteBrandRequest(Guid id) => Id = id;
}
