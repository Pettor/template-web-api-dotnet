using Backend.Application.Multitenancy.Entities;
using Backend.Application.Multitenancy.Interfaces;
using IValidator = Ardalis.Specification.IValidator;

namespace Backend.Application.Multitenancy.Queries.Get;

public class GetTenantRequestHandler(
    ITenantService tenantService,
    IValidator<GetTenantRequest> requestValidator
) : IRequestHandler<GetTenantRequest, TenantDto>
{
    public Task<TenantDto> Handle(GetTenantRequest request, CancellationToken cancellationToken)
    {
        requestValidator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
        return tenantService.GetByIdAsync(request.TenantId);
    }
}
