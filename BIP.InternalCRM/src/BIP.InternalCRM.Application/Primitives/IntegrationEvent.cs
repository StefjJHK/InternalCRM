using MediatR;

namespace BIP.InternalCRM.Application.Primitives;

public record IntegrationEvent(Guid Id) : INotification;