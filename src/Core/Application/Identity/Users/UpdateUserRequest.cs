using Backend.Application.Common.FileStorage;

namespace Backend.Application.Identity.Users;

public class UpdateUserRequest
{
    public string Id { get; } = default!;
    public string? FirstName { get; } = default!;
    public string? LastName { get; } = default!;
    public string? PhoneNumber { get; } = default!;
    public string? Email { get; } = default!;
    public FileUploadRequest? Image { get; } = default!;
    public bool DeleteCurrentImage { get; } = false;
}
