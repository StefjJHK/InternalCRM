namespace BIP.InternalCRM.WebIdentity.Permissions;

public record PaymentPermissionSet(
    bool? CanWrite,
    bool? CanRead
) : IPermissions
{
    public static readonly PaymentPermissionSet Empty = new(null, null);

    public static PaymentPermissionSet Create(bool? canWrite, bool? canRead)
        => new(canWrite, canRead);
}