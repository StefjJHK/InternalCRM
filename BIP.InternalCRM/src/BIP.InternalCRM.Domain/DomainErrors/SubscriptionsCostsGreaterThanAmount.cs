using BIP.InternalCRM.Domain.Subscriptions;

namespace BIP.InternalCRM.Domain.DomainErrors;

public record SubscriptionsCostsGreaterThanAmount() :
    DomainError(
        $"{nameof(Subscription)}.{nameof(SubscriptionsCostsGreaterThanAmount)}",
        "The costs of invoice subscriptions greater than amount of invoice");