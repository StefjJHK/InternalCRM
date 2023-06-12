using BIP.InternalCRM.WebIdentity.Permissions;
using BIP.InternalCRM.WebIdentity.Roles;
using Microsoft.AspNetCore.Identity;

namespace BIP.InternalCRM.WebIdentity.Users;

public sealed class User : IdentityUser<Guid>
{
    private User(
        UserId id,
        string username,
        PermissionSet permissions
    ) : base(username)
    {
        Id = id.Value;
        PermissionSet = permissions;

        _roles = new List<Role>();
    }
    
    #pragma warning disable
    // constructor for ef core
    private User() : base() { }
    #pragma warning restore

    public PermissionSet PermissionSet { get; set; }

    private readonly ICollection<Role> _roles;
    public IReadOnlyCollection<Role> Roles => _roles.ToList();

    public static User Create(
        UserId id,
        string username,
        string password,
        PermissionSet permissions)
    {
        var @new = new User(id, username, permissions);

        return @new;
    }
}
