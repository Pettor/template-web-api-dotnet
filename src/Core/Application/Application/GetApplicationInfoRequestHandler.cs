using System.Reflection;

namespace Backend.Application.Application;

public class GetApplicationInfoRequestHandler : IRequestHandler<GetApplicationInfoRequest, ApplicationInfoDto>
{
    public Task<ApplicationInfoDto> Handle(GetApplicationInfoRequest request, CancellationToken cancellationToken)
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
        return Task.FromResult(new ApplicationInfoDto { Version = version });
    }
}
