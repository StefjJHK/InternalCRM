using BIP.InternalCRM.WebIdentity.Permissions;

namespace BIP.InternalCRM.WebIdentity.Users.Dtos;

public record UserAddDto(
    string Username,
    string Password,
    PermissionSet Permissions
);