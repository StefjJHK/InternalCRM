using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.WebIdentity.Permissions;
using BIP.InternalCRM.WebIdentity.Roles;
using BIP.InternalCRM.WebIdentity.Users.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OneOf;
using OneOf.Types;
using NotFound = BIP.InternalCRM.Application.AppErrors.NotFound;

namespace BIP.InternalCRM.WebIdentity.Users;

public class UserService : UserManager<User>, IUserService
{
    private readonly JwtTokenOptions _tokenOptions;
    private readonly IRoleStore<Role> _roleStore;
    private readonly IMapper _mapper;

    public UserService(
        IUserStore<User> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserService> logger,
        IOptions<JwtTokenOptions> tokenOptionsAccessor,
        IRoleStore<Role> roleStore, IMapper mapper)
        : base(
            store,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger)
    {
        _roleStore = roleStore;
        _mapper = mapper;
        _tokenOptions = tokenOptionsAccessor.Value;
    }

    public async Task<OneOf<JwtSecurityToken, NotFound>> GenerateTokenAsync(
        string username,
        string password,
        CancellationToken cancellationToken)
    {
        var user = await Users
            .Where(_ => _.UserName == username)
            .Include(_ => _.PermissionSet)
            .Include(_ => _.Roles
                .OrderBy(r => r.Priority))
            .ThenInclude(_ => _.PermissionSet)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null) return new NotFound<User>();

        var passwordIsValid = await CheckPasswordAsync(user, password);
        if (!passwordIsValid) return new NotFound<User>();

        var perms = ResolvePermissions(user.Roles
            .Select(_ => _.PermissionSet)
            .Append(user.PermissionSet)
            .ToList());
        
        var secret = Encoding.UTF8.GetBytes(_tokenOptions.Secret);
        var symKey = new SymmetricSecurityKey(secret);
        var signingCredentials = new SigningCredentials(symKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new("iat", DateTime.UtcNow.Ticks.ToString()),
        };
        claims = claims.Concat(_tokenOptions.Audience
                .Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)))
            .ToList();

        var token = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            expires: DateTime.UtcNow.AddSeconds(_tokenOptions.ExpiresIn),
            claims: claims,
            signingCredentials: signingCredentials);
        
        token.Payload.Add("user", _mapper.Map<UserTokenDto>(user));
        token.Payload.Add("permissions", perms);

        return token;
    }

    public async Task<OneOf<User, NotFound<User>>> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        var user = await Users
            .Where(_ => _.UserName == username)
            .Include(_ => _.PermissionSet)
            .FirstOrDefaultAsync(cancellationToken);

        return user is not null
            ? user
            : new NotFound<User>();
    }

    public async Task<IReadOnlyCollection<Role>> GetUserRolesAsync(string username, CancellationToken cancellationToken = default)
    {
        var roles = await Users
            .AsNoTracking()
            .Where(_ => _.UserName == username)
            .SelectMany(_ => _.Roles)
            .ToListAsync(cancellationToken);

        return roles;
    }

    public async Task<OneOf<User, IReadOnlyCollection<IdentityError>>> AddUserAsync(string username, string password,
        PermissionSet permissionSet)
    {
        var user = User.Create(
            new UserId(Guid.NewGuid()),
            username,
            password,
            permissionSet);

        var result = await CreateAsync(user, password);

        return result.Succeeded ? user : result.Errors.ToList();
    }

    public async Task<IReadOnlyCollection<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task<OneOf<Success, NotFound<User>, NotFound<Role>, IReadOnlyCollection<IdentityError>>>
        AddUserToRoleAsync(
            string roleName,
            string username,
            CancellationToken cancellationToken = default)
    {
        var user = await FindByNameAsync(username);
        if (user is null)
        {
            return new NotFound<User>();
        }

        var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);
        if (role is null)
        {
            return new NotFound<Role>();
        }

        var result = await AddToRoleAsync(user, role.Name!);

        return result.Succeeded ? new Success() : result.Errors.ToList();
    }

    public async Task<OneOf<User, NotFound<User>, IReadOnlyCollection<IdentityError>>> UpdateAsync(
        string usernameIdentity,
        string username,
        PermissionSet permissionSet)
    {
        var user = await FindByNameAsync(usernameIdentity);

        if (user is null)
        {
            return new NotFound<User>();
        }

        user.PermissionSet = permissionSet;
        var result = await SetUserNameAsync(user, username);

        return result.Succeeded ? user : result.Errors.ToList();
    }

    public async Task<OneOf<User, NotFound<User>, IReadOnlyCollection<IdentityError>>> ChangePassword(
        string usernameIdentity,
        string oldPassword,
        string newPassword)
    {
        var user = await FindByNameAsync(usernameIdentity);

        if (user is null)
        {
            return new NotFound<User>();
        }

        var result = await ChangePasswordAsync(user, oldPassword, newPassword);

        return result.Succeeded ? user : result.Errors.ToList();
    }

    public async Task<OneOf<Success, NotFound<User>, IReadOnlyCollection<IdentityError>>> DeleteByUsernameAsync(
        string username)
    {
        var getUserResult = await GetByUsernameAsync(username);

        if (getUserResult.Value is not User)
        {
            return getUserResult.AsT1;
        }

        var deleteResult = await DeleteAsync(getUserResult.AsT0);

        return deleteResult.Succeeded ? new Success() : deleteResult.Errors.ToList();
    }

    private static PermissionSet ResolvePermissions(ICollection<PermissionSet> permsSets)
    {
        var resultPerms = PermissionSet.Empty;

        var setInfos = typeof(PermissionSet).GetProperties()
            .Where(p => p.PropertyType.IsAssignableTo(typeof(IPermissions)));

        foreach (var setInfo in setInfos)
        {
            var resultPermsSet = setInfo.GetValue(resultPerms);
            var permInfos = setInfo.PropertyType.GetProperties();

            foreach (var permInfo in permInfos)
            {
                bool? value = false;
                foreach (var perms in permsSets)
                {
                    var permsSet = setInfo.GetValue(perms);
                    value = (bool?)permInfo.GetValue(permsSet) ?? value;
                    permInfo.SetValue(resultPermsSet, value);
                }
            }
        }

        return resultPerms;
    }
}