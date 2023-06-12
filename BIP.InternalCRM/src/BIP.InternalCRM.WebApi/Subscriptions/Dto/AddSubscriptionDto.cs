namespace BIP.InternalCRM.WebApi.Subscriptions.Dto;

public record AddSubscriptionDto(
    string SubLegalEntity,
    decimal Cost,
    DateTime ValidFrom,
    DateTime ValidUntil
);
