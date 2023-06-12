using BIP.InternalCRM.Domain.Invoices;

namespace BIP.InternalCRM.Application.Invoices.QueryResults;

public record InvoiceQueryResult(
    Invoice Invoice,
    string ProductName,
    string CustomerName
);