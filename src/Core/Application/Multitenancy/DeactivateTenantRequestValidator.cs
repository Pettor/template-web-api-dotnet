using WebApiTemplate.Application.Common.Validation;

namespace WebApiTemplate.Application.Multitenancy;

public class DeactivateTenantRequestValidator : CustomValidator<DeactivateTenantRequest>
{
    public DeactivateTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}