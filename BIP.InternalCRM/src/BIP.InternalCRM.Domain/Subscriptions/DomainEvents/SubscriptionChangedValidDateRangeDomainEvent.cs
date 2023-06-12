using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Subscriptions.DomainEvents;

public record SubscriptionChangedValidDateRangeDomainEvent(
    Guid Id,
    SubscriptionId SubscriptionId,
    DateTime NewValidFrom,
    DateTime NewValidUntil,
    DateTime OldValidFrom,
    DateTime OldValidUntil
) : DomainEvent(Id);