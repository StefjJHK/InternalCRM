using BIP.InternalCRM.WebIdentity.Permissions;
using BIP.InternalCRM.WebIdentity.Users;
using Microsoft.AspNetCore.Identity;

namespace BIP.InternalCRM.WebIdentity.Roles;

public sealed class Role : IdentityRole<Guid>
{
    private Role(
        RoleId id,
        string roleName,
        uint priority,
        string color,
        PermissionSet permissions
    ) : base(roleName)
    {
        Id = id.Value;
        Priority = priority;
        Color = color;
        PermissionSet = permissions;

        _users = new List<User>();
    }
    
    
    #pragma warning disable
    // constructor for ef core
    private Role() : base() { }
    #pragma warning restore
    
    public uint Priority { get; private set; }

    public string Color { get; set; }

    public PermissionSet PermissionSet { get; set; }

    private readonly ICollection<User> _users;
    public IReadOnlyCollection<User> Users => _users.ToList();

    public static Role Create(
        RoleId id,
        string roleName,
        uint priority,
        string color,
        PermissionSet permissions)
    {
        var @new = new Role(id, roleName, priority, color, permissions);

        return @new;
    }

    public IEnumerable<Role> ChangePriority(uint newPriority, IList<Role> otherRoles)
    {
        Priority = newPriority;

        otherRoles = otherRoles
            .OrderBy(_ => _.Priority)
            .ToList();
        
        otherRoles.Insert((int)Priority, this);
        
        return otherRoles
            .OrderBy(_ => _.Priority)
            .ToList();
    }
}
