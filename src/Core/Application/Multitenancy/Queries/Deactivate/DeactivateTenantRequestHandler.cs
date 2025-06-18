using Backend.Application.Multitenancy.Interfaces;

namespace Backend.Application.Multitenancy.Queries.Deactivate;

public class DeactivateTenantRequestHandler(
    ITenantService tenantService,
    IValidator<DeactivateTenantRequest> requestValidator
) : IRequestHandler<DeactivateTenantRequest, string>
{
    public Task<string> Handle(DeactivateTenantRequest request, CancellationToken cancellationToken)
    {
        requestValidator.ValidateAndThrowAsync(request, cancellationToken);
        return tenantService.DeactivateAsync(request.TenantId);
    }
}
