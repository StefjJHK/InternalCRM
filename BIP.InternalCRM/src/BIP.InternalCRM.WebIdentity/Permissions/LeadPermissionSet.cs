namespace BIP.InternalCRM.WebIdentity.Permissions;

public record LeadPermissionSet(
    bool? CanWrite,
    bool? CanRead
) : IPermissions
{
    public static readonly LeadPermissionSet Empty = new(null, null);

    public static LeadPermissionSet Create(bool? canWrite, bool? canRead)
        => new(canWrite, canRead);
}