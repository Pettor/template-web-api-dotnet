using Backend.Application.Common.Validation;

namespace Backend.Application.Multitenancy.Queries.Deactivate;

public class DeactivateTenantRequestValidator : CustomValidator<DeactivateTenantRequest>
{
    public DeactivateTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}
