using Backend.Application.Dashboard.Entities;
using Backend.Application.Dashboard.Queries.Get;
using Backend.Infrastructure.Auth.Permissions;
using Backend.Shared.Authorization;

namespace Backend.Host.Controllers.Dashboard;

public class DashboardController : VersionNeutralApiController
{
    [HttpGet]
    [MustHavePermission(ApiAction.View, ApiResource.Dashboard)]
    [OpenApiOperation(
        "Get dashboard data including stats, chart data and recent transactions.",
        ""
    )]
    public Task<DashboardDataDto> GetDataAsync()
    {
        return Mediator.Send(new GetDashboardDataRequest());
    }
}
