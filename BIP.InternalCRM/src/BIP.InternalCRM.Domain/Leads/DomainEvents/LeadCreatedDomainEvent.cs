using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Leads.DomainEvents;

public record LeadCreatedDomainEvent(
    Guid Id,
    LeadId LeadId,
    Lead Lead
) : DomainEvent(Id);