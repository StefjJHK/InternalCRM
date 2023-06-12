namespace BIP.InternalCRM.WebApi.PurchaseOrders.Dto;

public record PurchaseOrderDto(
    string Number,
    decimal Amount,
    DateTime ReceivedDate,
    DateTime DueDate,
    DateTime? PaidDate,
    string ProductName,
    string CustomerName
);
