using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Subscriptions;

public readonly record struct SubscriptionId(Guid Value) : IStronglyTypedId;