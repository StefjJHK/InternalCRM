namespace BIP.InternalCRM.WebApi.Statistics.Payments;

public record PaymentSummaryStatisticsDto(
    int TotalPayments,
    decimal TotalAmount,
    int TotalOverdue
);