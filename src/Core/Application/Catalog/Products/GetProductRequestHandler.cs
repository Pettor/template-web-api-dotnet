using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Persistence;
using Backend.Domain.Catalog;

namespace Backend.Application.Catalog.Products;

public class GetProductRequestHandler : IRequestHandler<GetProductRequest, ProductDetailsDto>
{
    private readonly IRepository<Product> _repository;
    private readonly IStringLocalizer<GetProductRequestHandler> _localizer;

    public GetProductRequestHandler(IRepository<Product> repository, IStringLocalizer<GetProductRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<ProductDetailsDto> Handle(GetProductRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Product, ProductDetailsDto>)new ProductByIdWithBrandSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["product.notfound"], request.Id));
}
