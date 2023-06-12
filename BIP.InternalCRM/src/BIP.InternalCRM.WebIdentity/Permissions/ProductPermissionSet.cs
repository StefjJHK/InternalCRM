namespace BIP.InternalCRM.WebIdentity.Permissions;

public record ProductPermissionSet(
    bool? CanWrite,
    bool? CanRead
) : IPermissions
{
    public static readonly ProductPermissionSet Empty = new(null, null);

    public static ProductPermissionSet Create(bool? canWrite, bool? canRead)
        => new(canWrite, canRead);
}