using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.PurchaseOrders.DomainEvents;

public record PurchaseOrderCreatedDomainEvent(
    Guid Id,
    PurchaseOrderId PurchaseOrderId,
    PurchaseOrder PurchaseOrder
) : DomainEvent(Id);