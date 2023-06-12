using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Subscriptions.DomainEvents;

public record SubscriptionChangedNumberDomainEvent(
    Guid Id,
    SubscriptionId SubscriptionId,
    string NewNumber,
    string OldNumber
) : DomainEvent(Id); 