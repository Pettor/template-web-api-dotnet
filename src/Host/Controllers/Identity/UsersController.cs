using Backend.Application.Identity.Users;
using Backend.Application.Identity.Users.Password;
using Backend.Infrastructure.Auth.Permissions;
using Backend.Infrastructure.OpenApi;
using Backend.Shared.Authorization;
using FluentValidation;

namespace Backend.Host.Controllers.Identity;

public class UsersController : VersionNeutralApiController
{
    private readonly IValidator<CreateUserRequest> _createUserValidator;
    private readonly IValidator<ResetPasswordRequest> _resetPasswordValidator;
    private readonly IValidator<ForgotPasswordRequest> _forgotPasswordValidator;
    private readonly IUserService _userService;

    public UsersController(
        IValidator<CreateUserRequest> createUserValidator,
        IValidator<ResetPasswordRequest> resetPasswordValidator,
        IValidator<ForgotPasswordRequest> forgotPasswordValidator,
        IUserService userService)
    {
        _createUserValidator = createUserValidator;
        _resetPasswordValidator = resetPasswordValidator;
        _forgotPasswordValidator = forgotPasswordValidator;
        _userService = userService;
    }

    [HttpGet]
    [MustHavePermission(ApiAction.View, ApiResource.Users)]
    [OpenApiOperation("Get list of all users.", "")]
    public Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken)
    {
        return _userService.GetListAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    [MustHavePermission(ApiAction.View, ApiResource.Users)]
    [OpenApiOperation("Get a user's details.", "")]
    public Task<UserDetailsDto> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _userService.GetAsync(id, cancellationToken);
    }

    [HttpGet("{id}/roles")]
    [MustHavePermission(ApiAction.View, ApiResource.UserRoles)]
    [OpenApiOperation("Get a user's roles.", "")]
    public Task<List<UserRoleDto>> GetRolesAsync(string id, CancellationToken cancellationToken)
    {
        return _userService.GetRolesAsync(id, cancellationToken);
    }

    [HttpPost("{id}/roles")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    [MustHavePermission(ApiAction.Update, ApiResource.UserRoles)]
    [OpenApiOperation("Update a user's assigned roles.", "")]
    public Task<string> AssignRolesAsync(string id, UserRolesRequest request, CancellationToken cancellationToken)
    {
        return _userService.AssignRolesAsync(id, request, cancellationToken);
    }

    [HttpPost]
    [MustHavePermission(ApiAction.Create, ApiResource.Users)]
    [OpenApiOperation("Creates a new user.", "")]
    public async Task<string> CreateAsync(CreateUserRequest request)
    {
        await _createUserValidator.ValidateAndThrowAsync(request);

        // TODO: add other protection to prevent automatic posting (captcha?)
        return await _userService.CreateAsync(request, GetOriginFromRequest());
    }

    [HttpPost("self-register")]
    [TenantIdHeader]
    [AllowAnonymous]
    [OpenApiOperation("Anonymous user creates a user.", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public async Task<string> SelfRegisterAsync(CreateUserRequest request)
    {
        await _createUserValidator.ValidateAndThrowAsync(request);

        // TODO: add other protection to prevent automatic posting (captcha?)
        return await _userService.CreateAsync(request, GetOriginFromRequest());
    }

    [HttpPost("{id}/toggle-status")]
    [MustHavePermission(ApiAction.Update, ApiResource.Users)]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    [OpenApiOperation("Toggle a user's active status.", "")]
    public async Task<ActionResult> ToggleStatusAsync(string id, ToggleUserStatusRequest request, CancellationToken cancellationToken)
    {
        if (id != request.UserId)
        {
            return BadRequest();
        }

        await _userService.ToggleStatusAsync(request, cancellationToken);
        return Ok();
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    [OpenApiOperation("Confirm email address for a user.", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Search))]
    public Task<string> ConfirmEmailAsync([FromQuery] string tenant, [FromQuery] string userId, [FromQuery] string code, CancellationToken cancellationToken)
    {
        return _userService.ConfirmEmailAsync(userId, code, tenant, cancellationToken);
    }

    [HttpGet("confirm-phone-number")]
    [AllowAnonymous]
    [OpenApiOperation("Confirm phone number for a user.", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Search))]
    public Task<string> ConfirmPhoneNumberAsync([FromQuery] string userId, [FromQuery] string code)
    {
        return _userService.ConfirmPhoneNumberAsync(userId, code);
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request a pasword reset email for a user.", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        await _forgotPasswordValidator.ValidateAndThrowAsync(request);

        return await _userService.ForgotPasswordAsync(request, GetOriginFromRequest());
    }

    [HttpPost("reset-password")]
    [OpenApiOperation("Reset a user's password.", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
    {
        await _resetPasswordValidator.ValidateAndThrowAsync(request);

        return await _userService.ResetPasswordAsync(request);
    }

    private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
}
