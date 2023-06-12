using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Products;

public readonly record struct ProductId(Guid Value) : IStronglyTypedId;
    