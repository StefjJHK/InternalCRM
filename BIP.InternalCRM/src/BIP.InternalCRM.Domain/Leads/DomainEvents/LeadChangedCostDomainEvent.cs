using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Leads.DomainEvents;

public record LeadChangedCostDomainEvent(
    Guid Id,
    LeadId LeadId,
    decimal NewCost,
    decimal OldCost
) : DomainEvent(Id);