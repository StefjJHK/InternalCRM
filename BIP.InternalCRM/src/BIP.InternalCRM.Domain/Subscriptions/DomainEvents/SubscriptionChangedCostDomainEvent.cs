using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Subscriptions.DomainEvents;

public record SubscriptionChangedCostDomainEvent(
    Guid Id,
    SubscriptionId SubscriptionId,
    decimal NewCost,
    decimal OldCost
) : DomainEvent(Id);