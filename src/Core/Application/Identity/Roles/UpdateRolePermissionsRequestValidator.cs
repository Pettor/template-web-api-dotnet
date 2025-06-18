using Backend.Application.Common.Validation;

namespace Backend.Application.Identity.Roles;

public class UpdateRolePermissionsRequestValidator : CustomValidator<UpdateRolePermissionsRequest>
{
    public UpdateRolePermissionsRequestValidator()
    {
        RuleFor(r => r.RoleId).NotEmpty();
        RuleFor(r => r.Permissions).NotNull();
    }
}
