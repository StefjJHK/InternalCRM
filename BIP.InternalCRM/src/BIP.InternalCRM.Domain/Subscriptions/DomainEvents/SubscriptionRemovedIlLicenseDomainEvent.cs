using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Subscriptions.DomainEvents;

public record SubscriptionRemovedIlLicenseDomainEvent(
    Guid Id,
    SubscriptionId SubscriptionId,
    IntelliLockLicense License
) : DomainEvent(Id);