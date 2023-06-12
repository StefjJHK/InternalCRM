namespace BIP.InternalCRM.Application.FileStorage;

public interface IFileStorageService
{
    Task<byte[]> GetAsync(string filename, CancellationToken cancellationToken = default);

    Task SaveAsync(string filename, byte[] data, CancellationToken cancellationToken = default);

    Task DeleteAsync(string filename, CancellationToken cancellationToken = default);
}
