using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Products.DomainEvents;

public record ProductAddedIlProjectDomainEvent(
    Guid Id,
    ProductId ProductId,
    Product Product,
    IntelliLockProject Project
) : DomainEvent(Id);