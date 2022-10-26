namespace WebApiTemplate.Application.Multitenancy;

public class ActivateTenantRequest : IRequest<string>
{
    public string TenantId { get; set; } = default!;

    public ActivateTenantRequest(string tenantId) => TenantId = tenantId;
}