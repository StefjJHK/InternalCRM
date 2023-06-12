using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.WebIdentity.Permissions;
using BIP.InternalCRM.WebIdentity.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.WebIdentity.Roles;

public class RoleService : RoleManager<Role>, IRoleService
{
    public RoleService(
        IRoleStore<Role> store,
        IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RoleService> logger) : base(store,
        roleValidators,
        keyNormalizer,
        errors,
        logger)
    {
    }

    public async Task<OneOf<Role, NotFound<Role>>> GetRoleByName(
        string roleName,
        CancellationToken cancellationToken = default)
    {
        var role = await Roles
            .Where(_ => _.Name == roleName)
            .Include(_ => _.Users)
            .Include(_ => _.PermissionSet)
            .FirstOrDefaultAsync(cancellationToken);

        if (role is null)
        {
            return new NotFound<Role>();
        }

        return role;
    }

    public async Task<IReadOnlyCollection<Role>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        var roles = await Roles
            .AsNoTracking()
            .Include(_ => _.Users)
            .Include(_ => _.PermissionSet)
            .ToListAsync(cancellationToken);

        return roles;
    }

    public async Task<IReadOnlyCollection<User>> GetRoleUsersAsync(string roleName, CancellationToken cancellationToken)
    {
        var users = await Roles
            .AsNoTracking()
            .SelectMany(_ => _.Users)
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task<OneOf<Role, IReadOnlyCollection<IdentityError>>> AddRoleAsync(
        string name,
        uint priority,
        string color,
        PermissionSet permissionSet)
    {
        var role = Role.Create(
            new RoleId(Guid.NewGuid()),
            name,
            priority,
            color,
            permissionSet
        );

        var result = await CreateAsync(role);

        return result.Succeeded ? role : result.Errors.ToList();
    }

    public async Task<OneOf<Role, NotFound<Role>, IReadOnlyCollection<IdentityError>>> UpdateAsync(
        string roleNameIdentity,
        string roleName,
        uint priority,
        string color,
        PermissionSet permissionSet,
        CancellationToken cancellationToken = default)
    {
        var role = await Roles
            .Where(_ => _.Name == roleNameIdentity)
            .Include(_ => _.PermissionSet)
            .Include(_ => _.Users)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (role is null) return new NotFound<Role>();

        var otherRoles = await Roles.ToListAsync(cancellationToken);
            
        role.Name = roleName;
        role.Color = color;
        role.PermissionSet = permissionSet;
        var updatedRoles = role.ChangePriority(priority, otherRoles);

        foreach (var updatedRole in updatedRoles)
        {
            var result = await UpdateRoleAsync(updatedRole);

            if (!result.Succeeded)
            {
                return result.Errors.ToList();
            }
        }

        return role;
    }

    public async Task<OneOf<Success, NotFound<Role>, IReadOnlyCollection<IdentityError>>> DeleteAsync(string roleName)
    {
        var role = await FindByNameAsync(roleName);
        if (role is null)
        {
            return new NotFound<Role>();
        }

        var result = await DeleteAsync(role);

        return result.Succeeded ? new Success() : result.Errors.ToList();
    }
}