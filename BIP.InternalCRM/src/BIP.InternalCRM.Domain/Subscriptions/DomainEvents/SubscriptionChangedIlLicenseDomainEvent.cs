using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Subscriptions.DomainEvents;

public record SubscriptionChangedIlLicenseDomainEvent(
    Guid Id,
    SubscriptionId SubscriptionId,
    IntelliLockLicense NewIlLicense,
    IntelliLockLicense OldIlLicense
) : DomainEvent(Id);