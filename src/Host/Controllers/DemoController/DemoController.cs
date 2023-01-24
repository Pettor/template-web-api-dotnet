using Backend.Application.Demo;
using Backend.Infrastructure.Auth.Permissions;
using Backend.Shared.Authorization;

namespace Backend.Host.Controllers.DemoController;

public class DemoController : VersionedApiController
{
    [HttpGet]
    [MustHavePermission(ApiAction.View, ApiResource.Demo)]
    [OpenApiOperation("Used for demo purpose to receive some random data.", "")]
    public Task<DemoData> GetAsync()
    {
        return Mediator.Send(new GetDemoDataRequest());
    }
}
