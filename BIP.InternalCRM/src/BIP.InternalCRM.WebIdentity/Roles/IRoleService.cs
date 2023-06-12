using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.WebIdentity.Permissions;
using BIP.InternalCRM.WebIdentity.Users;
using Microsoft.AspNetCore.Identity;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.WebIdentity.Roles;

public interface IRoleService
{
    Task<OneOf<Role, NotFound<Role>>> GetRoleByName(
        string roleName,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Role>> GetAllRolesAsync(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<User>> GetRoleUsersAsync(string roleName, CancellationToken cancellationToken);

    Task<OneOf<Role, IReadOnlyCollection<IdentityError>>> AddRoleAsync(
        string name,
        uint priority,
        string color,
        PermissionSet permissionSet);

    Task<OneOf<Role, NotFound<Role>, IReadOnlyCollection<IdentityError>>> UpdateAsync(
        string roleNameIdentity,
        string roleName,
        uint priority,
        string color,
        PermissionSet permissionSet,
        CancellationToken cancellationToken = default);

    Task<OneOf<Success, NotFound<Role>, IReadOnlyCollection<IdentityError>>> DeleteAsync(string roleName);
}