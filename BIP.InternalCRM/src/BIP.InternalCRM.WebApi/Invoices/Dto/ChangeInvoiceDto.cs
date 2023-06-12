namespace BIP.InternalCRM.WebApi.Invoices.Dto;

public record ChangeInvoiceDto(
    string Number,
    decimal Amount,
    DateTime ReceivedDate,
    DateTime DueDate,
    string? PurchaseOrderNumber
);
