namespace BIP.InternalCRM.WebApi.Extensions;

public static class FormFileExtensions
{
    public static byte[] ReadBytes(this IFormFile file)
    {
        using var ms = new MemoryStream();
        using var fs = file.OpenReadStream();

        fs.CopyTo(ms);

        return ms.Length > 0
            ? ms.ToArray()
            : Array.Empty<byte>();
    }
}
