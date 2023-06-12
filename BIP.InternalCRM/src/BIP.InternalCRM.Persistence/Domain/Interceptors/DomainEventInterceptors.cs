using BIP.InternalCRM.Primitives.DomainDriven;
using CorrelationId;
using CorrelationId.Abstractions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace BIP.InternalCRM.Persistence.Domain.Interceptors;

public class DomainEventInterceptors : SaveChangesInterceptor
{
    private readonly CorrelationContext _correlationContext;

    public DomainEventInterceptors(ICorrelationContextAccessor correlationContextAccessor)
    {
        _correlationContext = correlationContextAccessor.CorrelationContext;
    }
    
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var domainEvents = dbContext!.ChangeTracker
            .Entries<Entity>()
            .Select(_ => _.Entity)
            .SelectMany(_ =>
            {
                var domainEvents = _.DomainEvents;
                _.CleanDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new DomainEventDbEntity(
                domainEvent.Id,
                DateTime.UtcNow,
                Guid.Parse(_correlationContext.CorrelationId),
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
            ))
            .ToList();

        await dbContext.Set<DomainEventDbEntity>().AddRangeAsync(domainEvents, cancellationToken);
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}