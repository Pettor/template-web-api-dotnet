namespace Backend.Application.Catalog.Brands;

public class GenerateRandomBrandRequest : IRequest<string>
{
    public int NSeed { get; set; }
}
