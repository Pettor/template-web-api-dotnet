using WebApiTemplate.Application.Common.Exporters;
using WebApiTemplate.Application.Common.Persistence;
using WebApiTemplate.Domain.Catalog;

namespace WebApiTemplate.Application.Catalog.Products;

public class ExportProductsRequestHandler : IRequestHandler<ExportProductsRequest, Stream>
{
    private readonly IReadRepository<Product> _repository;
    private readonly IExcelWriter _excelWriter;

    public ExportProductsRequestHandler(IReadRepository<Product> repository, IExcelWriter excelWriter)
    {
        _repository = repository;
        _excelWriter = excelWriter;
    }

    public async Task<Stream> Handle(ExportProductsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ExportProductsWithBrandsSpecification(request);

        var list = await _repository.ListAsync(spec, cancellationToken);

        return _excelWriter.WriteToStream(list);
    }
}