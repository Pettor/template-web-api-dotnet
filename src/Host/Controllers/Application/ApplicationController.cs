using Backend.Application.Application;

namespace Backend.Host.Controllers.Application;

public class ApplicationController : VersionNeutralApiController
{
    [HttpGet("info")]
    [AllowAnonymous]
    [OpenApiOperation("Get application information.", "")]
    public Task<ApplicationInfoDto> GetAsync()
    {
        return Mediator.Send(new GetApplicationInfoRequest());
    }
}
