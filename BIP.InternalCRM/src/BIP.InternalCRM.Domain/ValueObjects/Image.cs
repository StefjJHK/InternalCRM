using BIP.InternalCRM.Primitives.ValueObjects;

namespace BIP.InternalCRM.Domain.ValueObjects;

public record Image(
    string Filename
) : Document(Filename)
{
    private Image(string filename, byte[] data)
        : this(filename)
    {
        Data = data;
    }
    
    public string MediaType => $"image/{Path.GetExtension(Filename).Trim('.')}";

    public static Image Create(string filename, byte[] data)
    {
        var @new = new Image(filename, data);

        return @new;
    }
}