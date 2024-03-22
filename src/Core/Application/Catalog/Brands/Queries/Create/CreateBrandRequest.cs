namespace Backend.Application.Catalog.Brands.Queries.Create;

public class CreateBrandRequest : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}
