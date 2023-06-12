using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.PurchaseOrders.DomainEvents;

public record PurchaseOrderRemovedDomainEvent(
    Guid Id,
    PurchaseOrderId PurchaseOrderId,
    PurchaseOrder PurchaseOrder
) : DomainEvent(Id);