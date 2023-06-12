using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Invoices.DomainEvents;

public record InvoiceChangedNumberDomainEvent(
    Guid Id,
    InvoiceId InvoiceId,
    string NewNumber,
    string OldNumber
) : DomainEvent(Id);