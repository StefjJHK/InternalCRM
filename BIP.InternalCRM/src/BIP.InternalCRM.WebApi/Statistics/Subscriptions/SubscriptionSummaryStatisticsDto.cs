namespace BIP.InternalCRM.WebApi.Statistics.Subscriptions;

public record SubscriptionSummaryStatisticsDto(
    int TotalSubscriptions,
    decimal TotalCost,
    int NumberOfActive
);