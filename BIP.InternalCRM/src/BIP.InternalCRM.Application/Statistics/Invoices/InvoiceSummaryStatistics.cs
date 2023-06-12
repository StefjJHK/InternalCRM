namespace BIP.InternalCRM.Application.Statistics.Invoices;

public record InvoiceSummaryStatistics
{
    public int TotalInvoices { get; init; }
    
    public decimal TotalAmount { get; set; }
    
    public int NumberOfPaid { get; init; }
    
    public int NumberOfOverdue { get; init; }
}