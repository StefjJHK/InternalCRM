using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Payments.DomainEvents;

public record PaymentCreatedDomainEvent(
    Guid Id,
    PaymentId PaymentId,
    Payment Payment
) : DomainEvent(Id);
