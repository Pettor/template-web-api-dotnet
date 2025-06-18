using Backend.Application.Common.Validation;

namespace Backend.Application.Multitenancy.Queries.Active;

public class ActivateTenantRequestValidator : CustomValidator<ActivateTenantRequest>
{
    public ActivateTenantRequestValidator() => RuleFor(t => t.TenantId).NotEmpty();
}
