using MediatR;

namespace BIP.InternalCRM.Primitives.DomainDriven;

public abstract record DomainEvent(Guid Id) : INotification;
