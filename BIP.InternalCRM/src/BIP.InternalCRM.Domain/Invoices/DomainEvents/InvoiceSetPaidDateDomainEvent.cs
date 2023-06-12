using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Invoices.DomainEvents;

public record InvoiceSetPaidDateDomainEvent(
    Guid Id,
    InvoiceId InvoiceId,
    DateTime? NewPaidDate,
    DateTime? OldPaidDate
) : DomainEvent(Id);