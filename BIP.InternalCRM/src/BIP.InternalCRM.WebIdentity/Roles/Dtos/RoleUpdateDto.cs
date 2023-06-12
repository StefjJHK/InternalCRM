using BIP.InternalCRM.WebIdentity.Permissions;

namespace BIP.InternalCRM.WebIdentity.Roles.Dtos;

public record RoleUpdateDto(
    string RoleName,
    int Priority,
    string Color,
    PermissionSet PermissionSet
);