namespace BIP.InternalCRM.WebApi.Invoices.Dto;

public record ChangePaymentDto(
    string Number,
    decimal Amount,
    DateTime ReceivedDate
);
