namespace BIP.InternalCRM.Primitives.DomainDriven;

public interface IStronglyTypedId
{
    Guid Value { get; }
}