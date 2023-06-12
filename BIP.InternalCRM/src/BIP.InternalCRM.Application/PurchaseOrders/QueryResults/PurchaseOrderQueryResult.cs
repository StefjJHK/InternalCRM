using BIP.InternalCRM.Domain.PurchaseOrders;

namespace BIP.InternalCRM.Application.PurchaseOrders.QueryResults;

public record PurchaseOrderQueryResult(
    PurchaseOrder PurchaseOrder,
    string CustomerName,
    string ProductName
);