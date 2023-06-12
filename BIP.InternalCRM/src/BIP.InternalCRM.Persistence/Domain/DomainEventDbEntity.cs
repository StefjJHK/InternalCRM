namespace BIP.InternalCRM.Persistence.Domain;

public record DomainEventDbEntity(
    Guid EventId,
    DateTime OccurredOn,
    Guid CorrelationId,
    string EventType,
    string Content
);