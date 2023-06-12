using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Invoices.DomainEvents;

public record InvoiceChangedAmountDomainEvent(
     Guid Id,
     InvoiceId InvoiceId,
     decimal NewAmount,
     decimal OldAmount
) : DomainEvent(Id);