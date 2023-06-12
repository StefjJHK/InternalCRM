namespace BIP.InternalCRM.WebIdentity.Permissions;

public record SubscriptionPermissionSet(
    bool? CanWrite,
    bool? CanRead
) : IPermissions
{
    public static readonly SubscriptionPermissionSet Empty = new(null, null);

    public static SubscriptionPermissionSet Create(bool? canWrite, bool? canRead)
        => new(canWrite, canRead);
}