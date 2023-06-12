namespace BIP.InternalCRM.WebIdentity.Permissions;

public record CustomerPermissionSet(
    bool? CanWrite,
    bool? CanRead
) : IPermissions
{
    public static readonly CustomerPermissionSet Empty = new(null, null);

    public static CustomerPermissionSet Create(bool? canWrite, bool? canRead)
        => new(canWrite, canRead);
}