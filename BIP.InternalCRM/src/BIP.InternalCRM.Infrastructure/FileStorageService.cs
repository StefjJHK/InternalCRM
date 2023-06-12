using BIP.InternalCRM.Application.FileStorage;
using Microsoft.Extensions.Options;

namespace BIP.InternalCRM.Infrastructure;

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageOptions _options;

    public FileStorageService(IOptions<FileStorageOptions> optionsAccessor)
    {
        _options = optionsAccessor.Value;
    }

    public Task DeleteAsync(string filename, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fullPath = GetPath(filename);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public async Task<byte[]> GetAsync(string filename, CancellationToken cancellationToken = default)
    {
        var fullPath = GetPath(filename);

        return await File.ReadAllBytesAsync(fullPath, cancellationToken);
    }

    public async Task SaveAsync(string filename, byte[] data, CancellationToken cancellationToken = default)
    {
        var fullPath = GetPath(filename);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        using var stream = new MemoryStream(data);
        using var fs = new FileStream(fullPath, FileMode.OpenOrCreate);

        await stream.CopyToAsync(fs, cancellationToken);
    }

    private string GetPath(string filename)
        => Path.Combine(_options.Path, filename);
}
