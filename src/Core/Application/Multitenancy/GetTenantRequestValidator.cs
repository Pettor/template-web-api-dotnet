using WebApiTemplate.Application.Common.Validation;

namespace WebApiTemplate.Application.Multitenancy;

public class GetTenantRequestValidator : CustomValidator<GetTenantRequest>
{
    public GetTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}