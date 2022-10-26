using Backend.Application.Dashboard;
using Backend.Infrastructure.Auth.Permissions;
using Backend.Shared.Authorization;

namespace Backend.Host.Controllers.Dashboard;

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