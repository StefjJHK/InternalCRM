using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Customers.DomainEvents;

public record CustomerCreatedDomainEvent(
    Guid Id,
    CustomerId CustomerId,
    Customer Customer
) : DomainEvent(Id);