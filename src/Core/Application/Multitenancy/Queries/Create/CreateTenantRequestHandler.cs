using Backend.Application.Multitenancy.Interfaces;
using IValidator = Ardalis.Specification.IValidator;

namespace Backend.Application.Multitenancy.Queries.Create;

public class CreateTenantRequestHandler(
    ITenantService tenantService,
    IValidator<CreateTenantRequest> requestValidator
) : IRequestHandler<CreateTenantRequest, string>
{
    public Task<string> Handle(CreateTenantRequest request, CancellationToken cancellationToken)
    {
        requestValidator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
        return tenantService.CreateAsync(request, cancellationToken);
    }
}
