using Backend.Application.Multitenancy.Interfaces;
using IValidator = Ardalis.Specification.IValidator;

namespace Backend.Application.Multitenancy.Queries.Active;

public class ActivateTenantRequestHandler(
    ITenantService tenantService,
    IValidator<ActivateTenantRequest> tenantRequestValidator
) : IRequestHandler<ActivateTenantRequest, string>
{
    public Task<string> Handle(ActivateTenantRequest request, CancellationToken cancellationToken)
    {
        tenantRequestValidator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
        return tenantService.ActivateAsync(request.TenantId);
    }
}
