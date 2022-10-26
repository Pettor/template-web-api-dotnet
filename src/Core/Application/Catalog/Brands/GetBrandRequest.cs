namespace WebApiTemplate.Application.Catalog.Brands;

public class GetBrandRequest : IRequest<BrandDto>
{
    public Guid Id { get; set; }

    public GetBrandRequest(Guid id) => Id = id;
}