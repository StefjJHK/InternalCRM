using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Customers.DomainEvents;

public record CustomerChangeNameDomainEvent(
    Guid Id,
    CustomerId CustomerId,
    string NewName,
    string OldName
): DomainEvent(Id);