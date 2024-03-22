using Backend.Application.Catalog.Brands.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.Catalog.Brands.Queries.Generate;

public class GenerateRandomBrandRequestHandler : IRequestHandler<GenerateRandomBrandRequest, string>
{
    private readonly IJobService _jobService;

    public GenerateRandomBrandRequestHandler(IJobService jobService) => _jobService = jobService;

    public Task<string> Handle(GenerateRandomBrandRequest request, CancellationToken cancellationToken)
    {
        var jobId = _jobService.Enqueue<IBrandGeneratorJob>(x => x.GenerateAsync(request.NSeed, default));
        return Task.FromResult(jobId);
    }
}
