using System.IdentityModel.Tokens.Jwt;
using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.WebIdentity.Permissions;
using BIP.InternalCRM.WebIdentity.Roles;
using Microsoft.AspNetCore.Identity;
using OneOf;
using OneOf.Types;
using NotFound = BIP.InternalCRM.Application.AppErrors.NotFound;

namespace BIP.InternalCRM.WebIdentity.Users;

public interface IUserService
{
    Task<OneOf<JwtSecurityToken, NotFound>> GenerateTokenAsync(
        string username,
        string password,
        CancellationToken cancellationToken);
    
    Task<OneOf<User, NotFound<User>>> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Role>> GetUserRolesAsync(string username, CancellationToken cancellationToken = default);

    Task<OneOf<User, IReadOnlyCollection<IdentityError>>> AddUserAsync(
        string username,
        string password,
        PermissionSet permissionSet);

    Task<OneOf<Success, NotFound<User>, NotFound<Role>, IReadOnlyCollection<IdentityError>>> AddUserToRoleAsync(
        string roleName,
        string username,
        CancellationToken cancellationToken = default);

    Task<OneOf<User, NotFound<User>, IReadOnlyCollection<IdentityError>>> UpdateAsync(
        string usernameIdentity,
        string username,
        PermissionSet permissionSet);

    Task<OneOf<User, NotFound<User>, IReadOnlyCollection<IdentityError>>> ChangePassword(
        string usernameIdentity,
        string oldPassword,
        string newPassword);

    Task<OneOf<Success, NotFound<User>, IReadOnlyCollection<IdentityError>>> DeleteByUsernameAsync(string username);
}