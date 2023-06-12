using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Subscriptions.DomainEvents;

public record SubscriptionSetPaidDateDomainEvent(
    Guid Id,
    SubscriptionId SubscriptionId,
    DateTime? NewPaidDate,
    DateTime? OldPaidDate
) : DomainEvent(Id);