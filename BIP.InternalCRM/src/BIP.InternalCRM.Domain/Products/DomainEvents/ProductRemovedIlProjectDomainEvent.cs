using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Products.DomainEvents;

public record ProductRemovedIlProjectDomainEvent(
    Guid Id,
    ProductId ProductId,
    IntelliLockProject IlProject
) : DomainEvent(Id);