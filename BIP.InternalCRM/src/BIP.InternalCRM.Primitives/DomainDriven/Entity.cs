namespace BIP.InternalCRM.Primitives.DomainDriven;

public abstract class Entity
{
    protected Entity()
    {
        _domainEvents = new List<DomainEvent>();
    }
    
    private readonly ICollection<DomainEvent> _domainEvents;
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.ToList();

    protected void Raise(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void CleanDomainEvents() => _domainEvents.Clear();
}
public abstract class Entity<TStronglyTypedId> : Entity, IEquatable<Entity<TStronglyTypedId>>, ICloneable
    where TStronglyTypedId : IStronglyTypedId
{
    public TStronglyTypedId Id { get; }

    protected Entity(TStronglyTypedId id)
    {
        Id = id;
    }

    public static bool operator ==(Entity<TStronglyTypedId>? first, Entity<TStronglyTypedId>? second)
        => first is not null && second is not null && first.Equals(second);

    public static bool operator !=(Entity<TStronglyTypedId>? first, Entity<TStronglyTypedId>? second)
        => !(first == second);

    public bool Equals(Entity<TStronglyTypedId>? other)
    {
        if (other is null)
        {
            return false;
        }

        return other.GetType() == GetType() && other.Id.Equals(Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return obj is Entity<TStronglyTypedId> entity && entity.Id.Equals(Id);
    }

    public override int GetHashCode() => Id.GetHashCode() * 41;
    
    public abstract object Clone();
}