namespace BIP.InternalCRM.WebIdentity.Permissions;

public record StatisticsPermissionSet(
    bool? CanWrite,
    bool? CanRead
) : IPermissions
{
    public static readonly StatisticsPermissionSet Empty = new(null, null);

    public static StatisticsPermissionSet Create(bool? canWrite, bool? canRead)
        => new(canWrite, canRead);
}