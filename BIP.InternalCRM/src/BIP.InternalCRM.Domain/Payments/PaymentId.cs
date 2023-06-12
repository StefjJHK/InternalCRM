using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Payments;

public readonly record struct PaymentId(Guid Value) : IStronglyTypedId;