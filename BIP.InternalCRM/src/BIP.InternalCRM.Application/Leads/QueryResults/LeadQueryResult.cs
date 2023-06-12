using BIP.InternalCRM.Domain.Leads;

namespace BIP.InternalCRM.Application.Leads.QueryResults;

public record LeadQueryResult(Lead Lead, string ProductName);
