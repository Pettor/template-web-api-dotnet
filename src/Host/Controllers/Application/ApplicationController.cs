using Backend.Application.Application;
using Backend.Application.Application.Entities;
using Backend.Application.Application.Queries.Get;

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
