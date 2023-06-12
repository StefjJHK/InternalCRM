using BIP.InternalCRM.Domain.Payments;

namespace BIP.InternalCRM.Application.Payments.QueryResult;

public record PaymentQueryResult(
    Payment Payment,
    string InvoiceNumber
);
