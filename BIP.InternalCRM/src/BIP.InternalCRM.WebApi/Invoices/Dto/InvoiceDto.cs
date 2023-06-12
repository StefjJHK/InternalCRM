namespace BIP.InternalCRM.WebApi.Invoices.Dto;

public record InvoiceDto(
    string Number,
    decimal Amount,
    DateTime ReceivedDate,
    DateTime DueDate,
    DateTime? PaidDate,
    string? PurchaseOrderNumber,
    string CustomerName,
    string ProductName
);
