using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.PurchaseOrders.DomainEvents;

public record PurchaseOrderChangedExpirationDateDomainEvent(
    Guid Id,
    PurchaseOrderId PurchaseOrderId,
    DateTime NewReceivedDate,
    DateTime NewDueDate,
    DateTime OldReceivedDate,
    DateTime OldDueDate
) : DomainEvent(Id);