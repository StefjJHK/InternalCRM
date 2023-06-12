using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Domain.Subscriptions;
using OneOf;

namespace BIP.InternalCRM.Application.Subscriptions;

public interface IIntelliLockLicenseRepository
{
    Task<OneOf<IntelliLockLicense, NotFound>> GetBySubscriptionIdAsync(
        SubscriptionId productId,
        CancellationToken cancellationToken = default);
}