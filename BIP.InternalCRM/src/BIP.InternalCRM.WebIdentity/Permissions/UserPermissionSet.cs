namespace BIP.InternalCRM.WebIdentity.Permissions;

public record UserPermissionSet(
    bool? CanWrite,
    bool? CanRead)
{
    public static readonly UserPermissionSet Empty = new(null, null);
}