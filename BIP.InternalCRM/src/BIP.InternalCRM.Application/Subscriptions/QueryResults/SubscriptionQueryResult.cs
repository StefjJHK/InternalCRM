using BIP.InternalCRM.Domain.Subscriptions;

namespace BIP.InternalCRM.Application.Subscriptions.QueryResults;

public record SubscriptionQueryResult(
    Subscription Subscription,
    string InvoiceNumber
);