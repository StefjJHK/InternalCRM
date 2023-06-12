namespace BIP.InternalCRM.WebIdentity.Permissions;

public record RolePermissionSet(
    bool? CanWrite,
    bool? CanRead)
{
    public static readonly RolePermissionSet Empty = new(null, null);
}