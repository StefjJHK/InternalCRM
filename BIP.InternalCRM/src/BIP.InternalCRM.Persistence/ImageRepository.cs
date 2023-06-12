using BIP.InternalCRM.Application;
using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.FileStorage;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Persistence;

public class ImageRepository : IImageRepository
{
    private readonly string _filePathPattern;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDomainDbContext _domainDbContext;

    public ImageRepository(
        string filePathPattern,
        IFileStorageService fileStorageService,
        IDomainDbContext domainDbContext)
    {
        _filePathPattern = filePathPattern;
        _fileStorageService = fileStorageService;
        _domainDbContext = domainDbContext;
    }

    public async Task<OneOf<Image, NotFound>> GetIconByProductIdAsync(
        ProductId productId,
        CancellationToken cancellationToken = default)
    {
        var icon = await _domainDbContext.Products
            .AsNoTracking()
            .Where(_ => _.Id == productId)
            .Select(_ => _.Icon)
            .FirstOrDefaultAsync(cancellationToken);

        if (icon == null)
        {
            return new NotFound<Product>();
        }

        var data = await _fileStorageService.GetAsync(
            string.Format(_filePathPattern, icon.Filename),
            cancellationToken);
        icon.LoadData(data);

        return icon;
    }

    public async Task<OneOf<Image, NotFound>> GetImageByFilenameAsync(
        string filename,
        CancellationToken cancellationToken = default)
    {
        var image = await _domainDbContext.Products
            .AsNoTracking()
            .Select(_ => _.Icon)
            .Where(_ => _!.Filename == filename)
            .FirstOrDefaultAsync(cancellationToken);

        if (image is null) return new NotFound<Image>();

        var data = await _fileStorageService.GetAsync(
            string.Format(_filePathPattern, image.Filename),
            cancellationToken);
        image.LoadData(data);

        return image;
    }
}