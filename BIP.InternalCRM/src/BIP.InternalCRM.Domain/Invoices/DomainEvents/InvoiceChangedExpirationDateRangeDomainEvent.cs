using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Invoices.DomainEvents;

public record InvoiceChangedExpirationDateRangeDomainEvent(
    Guid Id,
    InvoiceId InvoiceId,
    DateTime NewReceivedDate,
    DateTime NewDueDate,
    DateTime OldReceivedDate,
    DateTime OldDueDate
) : DomainEvent(Id);