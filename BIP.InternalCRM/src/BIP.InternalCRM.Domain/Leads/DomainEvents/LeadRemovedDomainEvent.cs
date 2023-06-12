using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Leads.DomainEvents;

public record LeadRemovedDomainEvent(
    Guid Id,
    LeadId LeadId,
    Lead Lead
) : DomainEvent(Id);