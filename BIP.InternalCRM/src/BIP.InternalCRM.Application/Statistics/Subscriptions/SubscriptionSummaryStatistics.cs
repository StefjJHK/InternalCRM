namespace BIP.InternalCRM.Application.Statistics.Subscriptions;

public record SubscriptionSummaryStatistics
{
    public int TotalSubscriptions { get; set; }
    
    public decimal TotalCost { get; set; }
    
    public int NumberOfActive { get; set; }
}