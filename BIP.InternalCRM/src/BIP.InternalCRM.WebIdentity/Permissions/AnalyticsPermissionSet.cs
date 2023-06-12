namespace BIP.InternalCRM.WebIdentity.Permissions;

public record AnalyticsPermissionSet(
    bool? CanWrite,
    bool? CanRead
) : IPermissions
{
    public static readonly AnalyticsPermissionSet Empty = new(null, null);

    public static AnalyticsPermissionSet Create(bool? canWrite, bool? canRead)
        => new(canWrite, canRead);
}