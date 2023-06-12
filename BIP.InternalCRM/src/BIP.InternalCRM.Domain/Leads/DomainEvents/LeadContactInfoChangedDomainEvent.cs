using BIP.InternalCRM.Domain.ValueObjects;
using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Leads.DomainEvents;

public record LeadContactInfoChangedDomainEvent(
    Guid Id,
    LeadId LeadId,
    ContactInfo NewContactInfo,
    ContactInfo OldContactInfo
) : DomainEvent(Id);