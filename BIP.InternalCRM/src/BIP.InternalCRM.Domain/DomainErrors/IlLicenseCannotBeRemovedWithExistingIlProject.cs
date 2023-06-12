using BIP.InternalCRM.Domain.Subscriptions;

namespace BIP.InternalCRM.Domain.DomainErrors;

public record IlLicenseCannotBeRemovedWithExistingIlProject() :
    DomainError(
        $"{nameof(Subscription)}.{nameof(IlLicenseCannotBeRemovedWithExistingIlProject)}",
        "The IntelliLock license cannot be removed with existing IntelliLock project.");