using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.ValueObjects;
using OneOf;

namespace BIP.InternalCRM.Application;

public interface IImageRepository
{
    Task<OneOf<Image, NotFound>> GetIconByProductIdAsync(
        ProductId productId,
        CancellationToken cancellationToken = default);


    Task<OneOf<Image, NotFound>> GetImageByFilenameAsync(
        string filename,
        CancellationToken cancellationToken = default);
}