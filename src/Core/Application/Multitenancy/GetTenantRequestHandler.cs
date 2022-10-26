namespace WebApiTemplate.Application.Multitenancy;

public class GetTenantRequestHandler : IRequestHandler<GetTenantRequest, TenantDto>
{
    private readonly ITenantService _tenantService;

    public GetTenantRequestHandler(ITenantService tenantService) => _tenantService = tenantService;

    public Task<TenantDto> Handle(GetTenantRequest request, CancellationToken cancellationToken) =>
        _tenantService.GetByIdAsync(request.TenantId);
}