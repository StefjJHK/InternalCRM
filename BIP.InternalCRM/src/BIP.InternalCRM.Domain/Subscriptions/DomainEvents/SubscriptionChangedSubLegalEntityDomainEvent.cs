using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Subscriptions.DomainEvents;

public record SubscriptionChangedSubLegalEntityDomainEvent(
    Guid Id,
    SubscriptionId SubscriptionId,
    string NewSubLegalEntity,
    string OldSubLegalEntity
) : DomainEvent(Id);