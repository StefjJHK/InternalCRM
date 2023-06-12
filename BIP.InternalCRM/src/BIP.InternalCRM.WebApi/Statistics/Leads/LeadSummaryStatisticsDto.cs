namespace BIP.InternalCRM.WebApi.Statistics.Leads;

public record LeadSummaryStatisticsDto(
    int TotalLeads,
    int TotalProducts,
    decimal TotalCost
);