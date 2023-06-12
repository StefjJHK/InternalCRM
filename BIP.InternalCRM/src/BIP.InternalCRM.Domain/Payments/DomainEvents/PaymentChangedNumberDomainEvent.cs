using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Payments.DomainEvents;

public record PaymentChangedNumberDomainEvent(
    Guid Id,
    PaymentId PaymentId,
    string NewNumber,
    string OldNumber
) : DomainEvent(Id);