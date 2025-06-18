using Backend.Application.Common.Exporters;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Products.Queries.Export;

public class ExportProductsRequestHandler(
    IReadRepository<Product> repository,
    IExcelWriter excelWriter
) : IRequestHandler<ExportProductsRequest, Stream>
{
    public async Task<Stream> Handle(
        ExportProductsRequest request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ExportProductsWithBrandsSpecification(request);

        var list = await repository.ListAsync(spec, cancellationToken);

        return excelWriter.WriteToStream(list);
    }
}
