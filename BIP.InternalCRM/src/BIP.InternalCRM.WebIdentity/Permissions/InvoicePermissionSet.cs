namespace BIP.InternalCRM.WebIdentity.Permissions;

public record InvoicePermissionSet(
    bool? CanWrite,
    bool? CanRead
) : IPermissions
{
    public static readonly InvoicePermissionSet Empty = new(null, null);

    public static InvoicePermissionSet Create(bool? canWrite, bool? canRead)
        => new(canWrite, canRead);
}