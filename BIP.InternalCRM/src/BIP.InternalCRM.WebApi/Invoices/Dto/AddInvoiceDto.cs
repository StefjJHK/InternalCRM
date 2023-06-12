namespace BIP.InternalCRM.WebApi.Invoices.Dto;

#nullable enable

public record AddInvoiceDto(
    string Number,
    decimal Amount,
    DateTime ReceivedDate,
    DateTime DueDate,
    string? PurchaseOrderNumber,
    string ProductName,
    string CustomerName
);
