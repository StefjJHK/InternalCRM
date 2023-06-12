namespace BIP.InternalCRM.WebApi.Statistics.Invoices;

public record InvoiceSummaryStatisticsDto(
    int TotalInvoices,
    decimal TotalAmount,
    int NumberOfPaid,
    float PercentageOfPaid,
    int NumberOfOverdue,
    float PercentageOfOverdue
);