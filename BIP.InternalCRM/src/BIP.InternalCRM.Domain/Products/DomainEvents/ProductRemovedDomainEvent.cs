using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Products.DomainEvents;

public record ProductRemovedDomainEvent(
    Guid Id,
    ProductId ProductId,
    Product Product
) : DomainEvent(Id);