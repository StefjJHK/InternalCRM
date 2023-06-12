using BIP.InternalCRM.Domain.Subscriptions;
using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Invoices.DomainEvents;

public record InvoiceAddedSubscriptionDomainEvent(
    Guid Id,
    InvoiceId InvoiceId,
    Subscription Subscription
) : DomainEvent(Id);