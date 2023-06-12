namespace BIP.InternalCRM.WebApi.PurchaseOrders.Dto;

public record AddPurchaseOrderDto(
    string Number,
    decimal Amount,
    string CustomerName,
    string ProductName,
    DateTime ReceivedDate,
    DateTime DueDate
);