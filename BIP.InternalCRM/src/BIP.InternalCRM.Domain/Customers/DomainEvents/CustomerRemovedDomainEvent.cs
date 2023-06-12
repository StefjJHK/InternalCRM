using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Customers.DomainEvents;

public record CustomerRemovedDomainEvent(
    Guid Id,
    CustomerId CustomerId,
    Customer Customer
): DomainEvent(Id);