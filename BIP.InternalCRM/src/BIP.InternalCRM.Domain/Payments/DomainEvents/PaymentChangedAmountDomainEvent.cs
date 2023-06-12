using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Payments.DomainEvents;

public record PaymentChangedAmountDomainEvent(
    Guid Id,
    PaymentId PaymentId,
    decimal NewAmount,
    decimal OldAmount
) : DomainEvent(Id);