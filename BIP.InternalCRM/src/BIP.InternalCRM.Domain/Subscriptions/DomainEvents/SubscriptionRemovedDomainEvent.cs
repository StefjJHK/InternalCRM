using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Subscriptions.DomainEvents;

public record SubscriptionRemovedDomainEvent(
    Guid Id,
    SubscriptionId SubscriptionId,
    Subscription Subscription
) : DomainEvent(Id);