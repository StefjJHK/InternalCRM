namespace BIP.InternalCRM.WebApi.Subscriptions.Dto;

public record SubscriptionDto(
    string Number,
    string SubLegalEntity,
    decimal Cost,
    bool? IsPaid,
    DateTime ValidFrom,
    DateTime ValidUntil
);