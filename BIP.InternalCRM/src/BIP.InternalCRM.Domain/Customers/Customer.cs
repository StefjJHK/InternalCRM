using BIP.InternalCRM.Domain.Customers.DomainEvents;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.ValueObjects;
using BIP.InternalCRM.Primitives.DomainDriven;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.Domain.Customers;

public sealed class Customer : Entity<CustomerId>
{
    private Customer(
        CustomerId id,
        string name)
        : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public ContactInfo ContactInfo { get; private set; } = default!;

    public static OneOf<Customer, ValueMustBeUnique<Customer>> Create(
        CustomerId id,
        string name,
        IEnumerable<string> otherCustomersNames,
        ContactInfo contactInfo)
    {
        if (otherCustomersNames.Contains(name, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<Customer>(_ => _.Name);
        }

        var @new = new Customer(id, name) { ContactInfo = contactInfo };

        @new.Raise(new CustomerCreatedDomainEvent(Guid.NewGuid(), @new.Id, @new));

        return @new;
    }

    public Success Remove()
    {
        var removedCustomer = (Clone() as Customer)!;

        Raise(new CustomerRemovedDomainEvent(
            Guid.NewGuid(),
            Id,
            removedCustomer));

        return new Success();
    }

    public OneOf<Customer, ValueMustBeUnique<Customer>> ChangeName(
        string newName,
        IEnumerable<string> otherCustomersNames)
    {
        if (otherCustomersNames.Contains(newName, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<Customer>(_ => _.Name);
        }

        var oldName = (Name.Clone() as string)!;
        Name = newName;

        Raise(new CustomerChangeNameDomainEvent(
            Guid.NewGuid(),
            Id,
            newName,
            oldName));

        return this;
    }

    public Customer ChangeContactInfo(string fullname, string phoneNumber, string email)
    {
        var contactInfo = ContactInfo.Create(fullname, phoneNumber, email);

        Raise(new CustomerContactInfoChangedDomainEvent(
            Guid.NewGuid(),
            Id,
            contactInfo with { },
            ContactInfo with { }));

        ContactInfo = contactInfo;

        return this;
    }

    public override object Clone()
    {
        var cloned = new Customer(Id, Name)
        {
            ContactInfo = ContactInfo with { }
        };

        return cloned;
    }
}