using Backend.Application.Multitenancy.Interfaces;

namespace Backend.Application.Multitenancy.Queries.Create;

public class CreateTenantRequestHandler(ITenantService tenantService) : IRequestHandler<CreateTenantRequest, string>
{
    public Task<string> Handle(CreateTenantRequest request, CancellationToken cancellationToken) =>
        tenantService.CreateAsync(request, cancellationToken);
}
