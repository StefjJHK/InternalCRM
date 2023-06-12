using BIP.InternalCRM.Primitives.DomainDriven;

namespace BIP.InternalCRM.Domain.Payments.DomainEvents;

public record PaymentChangedReceivedDateDomainEvent(
    Guid Id,
    PaymentId PaymentId,
    DateTime NewDateTime,
    DateTime OldDateTime,
    bool NewIsOverdue,
    bool OldIsOverdue
) : DomainEvent(Id);