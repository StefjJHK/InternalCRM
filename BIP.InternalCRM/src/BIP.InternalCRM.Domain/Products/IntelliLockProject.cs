using BIP.InternalCRM.Primitives.ValueObjects;

namespace BIP.InternalCRM.Domain.Products;

public record IntelliLockProject(
    string Filename,
    string OriginalFilename
) : Document(Filename)
{
    private IntelliLockProject(
        string filename,
        string originalFilename,
        byte[] data)
        : this(filename, originalFilename)
    {
        Data = data;
    }
    
    public static IntelliLockProject Create(
        string filename,
        string originalFilename,
        byte[] data)
    {
        var @new = new IntelliLockProject(filename, originalFilename, data);

        return @new;
    }
}