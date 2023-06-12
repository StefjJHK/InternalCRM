namespace BIP.InternalCRM.WebApi.Statistics.Products;

public record ProductStatisticsDto(
    string ProductName,
    int TotalCustomers,
    int TotalSubscriptions,
    decimal TotalRevenue
);
