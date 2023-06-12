using BIP.InternalCRM.Domain.PurchaseOrders;
using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Invoices.DomainEvents;

public record InvoiceSetPurchaseOrderDomainEvent(
    Guid Id,
    InvoiceId InvoiceId,
    PurchaseOrder PurchaseOrder
) : DomainEvent(Id);