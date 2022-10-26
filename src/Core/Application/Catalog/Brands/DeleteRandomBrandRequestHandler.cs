﻿using WebApiTemplate.Application.Common.Interfaces;

namespace WebApiTemplate.Application.Catalog.Brands;

public class DeleteRandomBrandRequestHandler : IRequestHandler<DeleteRandomBrandRequest, string>
{
    private readonly IJobService _jobService;

    public DeleteRandomBrandRequestHandler(IJobService jobService) => _jobService = jobService;

    public Task<string> Handle(DeleteRandomBrandRequest request, CancellationToken cancellationToken)
    {
        string jobId = _jobService.Schedule<IBrandGeneratorJob>(x => x.CleanAsync(default), TimeSpan.FromSeconds(5));
        return Task.FromResult(jobId);
    }
}