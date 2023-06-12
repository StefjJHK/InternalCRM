using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Leads.DomainEvents;

public record LeadChangedExpirationDateRangeDomainEvent(
    Guid Id,
    LeadId LeadId,
    DateTime NewStartDate,
    DateTime NewEndDate,
    DateTime OldStartDate,
    DateTime OldEndDate
) : DomainEvent(Id);
