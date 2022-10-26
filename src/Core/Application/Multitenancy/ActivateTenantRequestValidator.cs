using WebApiTemplate.Application.Common.Validation;

namespace WebApiTemplate.Application.Multitenancy;

public class ActivateTenantRequestValidator : CustomValidator<ActivateTenantRequest>
{
    public ActivateTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}