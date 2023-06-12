using BIP.InternalCRM.Domain.ValueObjects;
using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Customers.DomainEvents;

public record CustomerContactInfoChangedDomainEvent(
    Guid Id,
    CustomerId CustomerId,
    ContactInfo NewContactInfo,
    ContactInfo OldContactInfo
) : DomainEvent(Id);