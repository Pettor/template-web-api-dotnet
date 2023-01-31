using Backend.Application.Auditing;
using Backend.Application.Identity.Users;
using Backend.Application.Identity.Users.Password;
using Backend.Shared.Authorization;
using FluentValidation;

namespace Backend.Host.Controllers.Personal;

public class PersonalController : VersionNeutralApiController
{
    private readonly IValidator<UpdateUserRequest> _updateUserValidator;
    private readonly IValidator<ChangePasswordRequest> _changePasswordValidator;
    private readonly IUserService _userService;

    public PersonalController(
        IValidator<UpdateUserRequest> updateUserValidator,
        IValidator<ChangePasswordRequest> changePasswordValidator,
        IUserService userService)
    {
        _updateUserValidator = updateUserValidator;
        _changePasswordValidator = changePasswordValidator;
        _userService = userService;
    }

    [HttpGet("profile")]
    [OpenApiOperation("Get profile details of currently logged in user.", "")]
    public async Task<ActionResult<UserDetailsDto>> GetProfileAsync(CancellationToken cancellationToken)
    {
        return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
            ? Unauthorized()
            : Ok(await _userService.GetAsync(userId, cancellationToken));
    }

    [HttpPut("profile")]
    [OpenApiOperation("Update profile details of currently logged in user.", "")]
    public async Task<ActionResult> UpdateProfileAsync(UpdateUserRequest request)
    {
        await _updateUserValidator.ValidateAndThrowAsync(request);

        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _userService.UpdateAsync(request, userId);
        return Ok();
    }

    [HttpPut("change-password")]
    [OpenApiOperation("Change password of currently logged in user.", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        await _changePasswordValidator.ValidateAndThrowAsync(request);

        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await _userService.ChangePasswordAsync(request, userId);
        return Ok();
    }

    [HttpGet("permissions")]
    [OpenApiOperation("Get permissions of currently logged in user.", "")]
    public async Task<ActionResult<List<string>>> GetPermissionsAsync(CancellationToken cancellationToken)
    {
        return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
            ? Unauthorized()
            : Ok(await _userService.GetPermissionsAsync(userId, cancellationToken));
    }

    [HttpGet("logs")]
    [OpenApiOperation("Get audit logs of currently logged in user.", "")]
    public Task<List<AuditDto>> GetLogsAsync()
    {
        return Mediator.Send(new GetMyAuditLogsRequest());
    }
}
