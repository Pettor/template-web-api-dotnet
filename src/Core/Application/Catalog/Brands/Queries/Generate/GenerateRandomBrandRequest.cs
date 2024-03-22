namespace Backend.Application.Catalog.Brands.Queries.Generate;

public class GenerateRandomBrandRequest : IRequest<string>
{
    public int NSeed { get; set; }
}
