﻿using Ardalis.Specification;
using Backend.Application.Auditing.Entities;
using Backend.Application.Auditing.Queries.Get;
using Backend.Application.Identity.Users;
using Backend.Application.Identity.Users.Password;
using Backend.Shared.Authorization;
using FluentValidation;

namespace Backend.Host.Controllers.Personal;

public class PersonalController(
    IUserService userService,
    IValidator<ChangePasswordRequest> changePasswordRequestValidator,
    IValidator<UpdateUserRequest> updateUserRequestValidator
) : VersionNeutralApiController
{
    [HttpGet("profile")]
    [OpenApiOperation("Get profile details of currently logged in user.", "")]
    public async Task<ActionResult<UserDetailsDto>> GetProfileAsync(
        CancellationToken cancellationToken
    )
    {
        return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
            ? Unauthorized()
            : Ok(await userService.GetAsync(userId, cancellationToken));
    }

    [HttpPut("profile")]
    [OpenApiOperation("Update profile details of currently logged in user.", "")]
    public async Task<ActionResult> UpdateProfileAsync(UpdateUserRequest request)
    {
        await updateUserRequestValidator.ValidateAndThrowAsync(request);
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await userService.UpdateAsync(request, userId);
        return Ok();
    }

    [HttpPut("change-password")]
    [OpenApiOperation("Change password of currently logged in user.", "")]
    [ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Register))]
    public async Task<ActionResult> ChangePasswordAsync(ChangePasswordRequest model)
    {
        await changePasswordRequestValidator.ValidateAndThrowAsync(model);
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        await userService.ChangePasswordAsync(model, userId);
        return Ok();
    }

    [HttpGet("permissions")]
    [OpenApiOperation("Get permissions of currently logged in user.", "")]
    public async Task<ActionResult<List<string>>> GetPermissionsAsync(
        CancellationToken cancellationToken
    )
    {
        return User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId)
            ? Unauthorized()
            : Ok(await userService.GetPermissionsAsync(userId, cancellationToken));
    }

    [HttpGet("logs")]
    [OpenApiOperation("Get audit logs of currently logged in user.", "")]
    public Task<List<AuditDto>> GetLogsAsync()
    {
        return Mediator.Send(new GetMyAuditLogsRequest());
    }
}
