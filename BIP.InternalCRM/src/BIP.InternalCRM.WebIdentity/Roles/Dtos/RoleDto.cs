using BIP.InternalCRM.WebIdentity.Users.Dtos;

namespace BIP.InternalCRM.WebIdentity.Roles.Dtos;

public record RoleDto(
    string Name, 
    int Priority,
    UserDto[] Users
);