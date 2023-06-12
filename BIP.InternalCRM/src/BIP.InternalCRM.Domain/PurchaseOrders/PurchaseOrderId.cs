using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.PurchaseOrders;

public readonly record struct PurchaseOrderId (Guid Value) : IStronglyTypedId;
