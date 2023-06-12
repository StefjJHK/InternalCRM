using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Customers;

public readonly record struct CustomerId(Guid Value) : IStronglyTypedId;