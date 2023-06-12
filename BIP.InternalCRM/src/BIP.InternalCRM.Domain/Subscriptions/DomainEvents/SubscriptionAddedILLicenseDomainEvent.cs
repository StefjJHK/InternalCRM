using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Subscriptions.DomainEvents;

public record SubscriptionAddedIlLicenseDomainEvent(
    Guid Id,
    SubscriptionId SubscriptionId,
    Subscription Subscription,
    IntelliLockLicense License
) : DomainEvent(Id);