using BIP.InternalCRM.Primitives.ValueObjects;

namespace BIP.InternalCRM.Domain.Subscriptions;

public record IntelliLockLicense(
    Guid Key,
    string Filename
) : Document(Filename)
{
    public static IntelliLockLicense Create(
        Guid key,
        string filename,
        byte[] data)
    {
        var @new = new IntelliLockLicense(key, filename)
        {
            Data = data
        };

        return @new;
    }
}