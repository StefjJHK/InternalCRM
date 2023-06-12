using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Invoices.DomainEvents;

public record InvoiceCreatedDomainEvent(
    Guid Id,
    InvoiceId InvoiceId,
    Invoice Invoice
) : DomainEvent(Id);