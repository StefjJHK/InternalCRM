using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Invoices;

public readonly record struct InvoiceId(Guid Value) : IStronglyTypedId;