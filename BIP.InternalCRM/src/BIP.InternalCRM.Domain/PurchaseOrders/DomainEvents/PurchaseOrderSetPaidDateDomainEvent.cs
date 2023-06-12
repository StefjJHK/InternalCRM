using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.PurchaseOrders.DomainEvents;

public record PurchaseOrderSetPaidDateDomainEvent(
    Guid Id,
    PurchaseOrderId PurchaseOrderId,
    DateTime? NewPaidDate,
    DateTime? OldPaidDate
) : DomainEvent(Id);