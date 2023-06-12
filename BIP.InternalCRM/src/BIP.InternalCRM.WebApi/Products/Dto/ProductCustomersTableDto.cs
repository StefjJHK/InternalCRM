namespace BIP.InternalCRM.WebApi.Products.Dto;

public record ProductCustomersTableDto(
    ProductCustomersTableRowDto[] Rows
);

public record ProductCustomersTableRowDto(
    string CustomerName,
    int TotalPurchaseOrders,
    int TotalInvoices,
    int TotalSubscriptions,
    decimal TotalRevenue
);