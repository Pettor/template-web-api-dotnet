﻿using WebApiTemplate.Application.Common.Interfaces;

namespace WebApiTemplate.Application.Catalog.Brands;

public class GenerateRandomBrandRequestHandler : IRequestHandler<GenerateRandomBrandRequest, string>
{
    private readonly IJobService _jobService;

    public GenerateRandomBrandRequestHandler(IJobService jobService) => _jobService = jobService;

    public Task<string> Handle(GenerateRandomBrandRequest request, CancellationToken cancellationToken)
    {
        string jobId = _jobService.Enqueue<IBrandGeneratorJob>(x => x.GenerateAsync(request.NSeed, default));
        return Task.FromResult(jobId);
    }
}