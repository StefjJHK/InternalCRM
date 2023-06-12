using BIP.InternalCRM.Domain.Payments;
using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Invoices.DomainEvents;

public record InvoiceAddedPaymentDomainEvent(
    Guid Id,
    InvoiceId InvoiceId,
    Payment Payment
) : DomainEvent(Id);