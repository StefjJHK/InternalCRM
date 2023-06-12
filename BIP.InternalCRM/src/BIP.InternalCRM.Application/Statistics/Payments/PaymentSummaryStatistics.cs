namespace BIP.InternalCRM.Application.Statistics.Payments;

public record PaymentSummaryStatistics
{
    public int TotalPayments { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public int TotalOverdue { get; set; }
}