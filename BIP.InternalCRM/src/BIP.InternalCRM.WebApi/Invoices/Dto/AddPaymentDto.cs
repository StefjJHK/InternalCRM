namespace BIP.InternalCRM.WebApi.Payments.Dtos;

public record AddPaymentDto(
    string Number,
    decimal Amount,
    DateTime ReceivedDate
);
