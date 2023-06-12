using BIP.InternalCRM.WebIdentity.Permissions;

namespace BIP.InternalCRM.WebIdentity.Users.Dtos;

public record UserUpdateDto(string Username, PermissionSet PermissionSet);