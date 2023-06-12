using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Domain.Products;
using OneOf;

namespace BIP.InternalCRM.Application.Products;

public interface IIntelliLockProjectRepository
{
    Task<OneOf<IntelliLockProject, NotFound>> GetByProductIdAsync(
        ProductId productId,
        CancellationToken cancellationToken = default);
}