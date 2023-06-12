using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.PurchaseOrders.DomainEvents;

public record PurchaseOrderChangedNumberDomainEvent(
    Guid Id,
    PurchaseOrderId PurchaseOrderId,
    string NewNumber,
    string OldNumber
) : DomainEvent(Id);