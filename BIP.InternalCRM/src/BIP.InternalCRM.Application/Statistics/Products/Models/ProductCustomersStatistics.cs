namespace BIP.InternalCRM.Application.Statistics.Products.Models;

public record ProductCustomersStatistics(
    string ProductName,
    string CustomerName
)
{
    public int TotalInvoices { get; set; }
    
    public int TotalPurchaseOrders { get; set; }
    
    public int TotalSubscriptions { get; set; }
    
    public decimal TotalRevenue { get; set; }
}