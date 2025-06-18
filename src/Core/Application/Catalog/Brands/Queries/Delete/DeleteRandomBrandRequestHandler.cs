using Backend.Application.Catalog.Brands.Interfaces;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.Catalog.Brands.Queries.Delete;

public class DeleteRandomBrandRequestHandler(IJobService jobService)
    : IRequestHandler<DeleteRandomBrandRequest, string>
{
    public Task<string> Handle(
        DeleteRandomBrandRequest request,
        CancellationToken cancellationToken
    )
    {
        var jobId = jobService.Schedule<IBrandGeneratorJob>(
            x => x.CleanAsync(default),
            TimeSpan.FromSeconds(5)
        );
        return Task.FromResult(jobId);
    }
}
