namespace BIP.InternalCRM.WebIdentity.Permissions;

public record AdminPermissionSet(
    bool? CanWrite,
    bool? CanRead)
{
    public static readonly AdminPermissionSet Empty = new(null, null);

}