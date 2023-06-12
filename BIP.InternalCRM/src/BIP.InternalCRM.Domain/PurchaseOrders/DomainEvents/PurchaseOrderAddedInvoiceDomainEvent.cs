using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.PurchaseOrders.DomainEvents;

public record PurchaseOrderAddedInvoiceDomainEvent(
    Guid Id,
    PurchaseOrderId PurchaseOrderId,
    Invoice Invoice
) : DomainEvent(Id);