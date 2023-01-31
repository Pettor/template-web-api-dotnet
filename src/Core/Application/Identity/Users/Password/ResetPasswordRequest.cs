namespace Backend.Application.Identity.Users.Password;

public class ResetPasswordRequest
{
    public string Email { get; } = default!;

    public string? Password { get; } = default!;

    public string? Token { get; } = default!;
}
