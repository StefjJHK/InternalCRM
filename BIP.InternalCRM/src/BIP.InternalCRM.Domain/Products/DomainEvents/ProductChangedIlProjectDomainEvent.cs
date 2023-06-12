using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Products.DomainEvents;

public record ProductChangedIlProjectDomainEvent(
    Guid Id,
    ProductId ProductId,
    IntelliLockProject NewIlProject,
    IntelliLockProject OldIlProject
) : DomainEvent(Id);