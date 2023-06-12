namespace BIP.InternalCRM.Application.Statistics.Products.Models;

public record ProductStatistics(
    string ProductName
)
{
    public int TotalCustomers { get; set; }

    public int TotalSubscriptions { get; set; }

    public decimal TotalRevenue { get; set; }
}