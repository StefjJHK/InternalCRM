using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Leads.DomainEvents;

public record LeadChangedNameDomainEvent(
    Guid Id,
    LeadId LeadId,
    string NewName,
    string OldName
) : DomainEvent(Id);