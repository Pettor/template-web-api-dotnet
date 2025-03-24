using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;

namespace Backend.Application.Common.FileStorage;

public interface IFileStorageService : ITransientService
{
    Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class;

    void Remove(string? path);
}
