namespace BIP.InternalCRM.WebIdentity.Permissions;

public record PurchaseOrderPermissionSet(
    bool? CanWrite,
    bool? CanRead
) : IPermissions
{
    public static readonly PurchaseOrderPermissionSet Empty = new(null, null);

    public static PurchaseOrderPermissionSet Create(bool? canWrite, bool? canRead)
        => new(canWrite, canRead);
}