namespace WebApiTemplate.Application.Identity.Users.Password;

public class ForgotPasswordRequest
{
    public string Email { get; set; } = default!;
}