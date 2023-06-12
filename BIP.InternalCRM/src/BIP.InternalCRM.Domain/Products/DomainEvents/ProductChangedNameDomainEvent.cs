using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Products.DomainEvents;

public record ProductChangedNameDomainEvent(
    Guid Id,
    ProductId ProductId,
    string NewName,
    string OldName
) : DomainEvent(Id);