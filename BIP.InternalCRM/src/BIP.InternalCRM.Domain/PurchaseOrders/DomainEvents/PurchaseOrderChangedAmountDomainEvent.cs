using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.PurchaseOrders.DomainEvents;

public record PurchaseOrderChangedAmountDomainEvent(
    Guid Id,
    PurchaseOrderId PurchaseOrderId,
    decimal NewAmount,
    decimal OldAmount
) : DomainEvent(Id);