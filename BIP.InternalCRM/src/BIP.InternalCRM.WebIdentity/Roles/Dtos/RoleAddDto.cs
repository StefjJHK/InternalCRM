using BIP.InternalCRM.WebIdentity.Permissions;

namespace BIP.InternalCRM.WebIdentity.Roles.Dtos;

public record RoleAddDto(
    string Name,
    int Priority,
    string Color,
    PermissionSet Permissions
);