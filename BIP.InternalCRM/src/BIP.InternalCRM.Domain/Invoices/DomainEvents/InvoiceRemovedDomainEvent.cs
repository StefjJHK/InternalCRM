using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Invoices.DomainEvents;

public record  InvoiceRemovedDomainEvent(
    Guid Id,
    InvoiceId InvoiceId,
    Invoice Invoice
) : DomainEvent(Id);