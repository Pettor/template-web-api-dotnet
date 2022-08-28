using WebApiTemplate.Application.Dashboard;
using WebApiTemplate.Infrastructure.Auth.Permissions;
using WebApiTemplate.Shared.Authorization;

namespace WebApiTemplate.Host.Controllers.Dashboard;

public class DashboardController : VersionedApiController
{
    [HttpGet]
    [MustHavePermission(FshAction.View, FshResource.Dashboard)]
    [OpenApiOperation("Get statistics for the dashboard.", "")]
    public Task<StatsDto> GetAsync()
    {
        return Mediator.Send(new GetStatsRequest());
    }
}