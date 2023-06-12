using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Leads;

public readonly record struct LeadId(Guid Value) : IStronglyTypedId;
