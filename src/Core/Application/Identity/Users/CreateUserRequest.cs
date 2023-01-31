namespace Backend.Application.Identity.Users;

public class CreateUserRequest
{
    public string FirstName { get; } = default!;
    public string LastName { get; } = default!;
    public string Email { get; } = default!;
    public string UserName { get; } = default!;
    public string Password { get; } = default!;
    public string ConfirmPassword { get; } = default!;
    public string? PhoneNumber { get; } = default!;
}
