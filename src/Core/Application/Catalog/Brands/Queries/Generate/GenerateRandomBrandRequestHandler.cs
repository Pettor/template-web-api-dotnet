using Backend.Application.Catalog.Brands.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.Catalog.Brands.Queries.Generate;

public class GenerateRandomBrandRequestHandler(IJobService jobService) : IRequestHandler<GenerateRandomBrandRequest, string>
{
    public Task<string> Handle(GenerateRandomBrandRequest request, CancellationToken cancellationToken)
    {
        var jobId = jobService.Enqueue<IBrandGeneratorJob>(x => x.GenerateAsync(request.NSeed, default));
        return Task.FromResult(jobId);
    }
}
