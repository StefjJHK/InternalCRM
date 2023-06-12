namespace BIP.InternalCRM.WebApi.Payments.Dtos;

public record PaymentDto(
    string Number,
    decimal Amount,
    DateTime ReceivedDate,
    string InvoiceNumber
);
