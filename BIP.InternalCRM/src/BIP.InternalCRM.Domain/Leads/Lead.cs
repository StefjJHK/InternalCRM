using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Leads.DomainEvents;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.ValueObjects;
using BIP.InternalCRM.Primitives.DomainDriven;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.Domain.Leads;

public sealed class Lead : Entity<LeadId>
{
    private Lead(
        LeadId id,
        string name,
        ProductId productId,
        decimal cost,
        DateTime startDate,
        DateTime endDate)
        : base(id)
    {
        Name = name;
        ProductId = productId;
        Cost = cost;
        StartDate = startDate;
        EndDate = endDate;
    }

    public string Name { get; private set; }

    public ContactInfo ContactInfo { get; private set; } = default!;

    public ProductId ProductId { get; }

    public decimal Cost { get; private set; }

    public DateTime StartDate { get; private set; }

    public DateTime EndDate { get; private set; }

    public static OneOf<Lead, ValueMustBeUnique<Lead>> Create(
        LeadId id,
        string name,
        IEnumerable<string> otherLeadsNames,
        ContactInfo contactInfo,
        ProductId productId,
        decimal cost,
        DateTime startDate,
        DateTime endDate)
    {
        if (otherLeadsNames.Contains(name, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<Lead>(_ => _.Name);
        }

        var @new = new Lead(id, name, productId, cost, startDate, endDate) { ContactInfo = contactInfo };

        @new.Raise(new LeadCreatedDomainEvent(Guid.NewGuid(), @new.Id, @new));

        return @new;
    }

    public Success Remove()
    {
        var removedLead = (Clone() as Lead)!;

        Raise(new LeadRemovedDomainEvent(
            Guid.NewGuid(),
            Id,
            removedLead));

        return new Success();
    }

    public OneOf<Lead, ValueMustBeUnique<Lead>> ChangeName(string newName, IEnumerable<string> otherLeadsNames)
    {
        if (otherLeadsNames.Contains(newName, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<Lead>(_ => _.Name);
        }

        var oldName = (Name.Clone() as string)!;
        Name = newName;

        Raise(new LeadChangedNameDomainEvent(
            Guid.NewGuid(),
            Id,
            newName,
            oldName));

        return this;
    }

    public Lead ChangeContactInfo(string fullname, string phoneNumber, string email)
    {
        var newContactInfo = ContactInfo.Create(fullname, phoneNumber, email);

        var oldContactInfo = ContactInfo with { };
        ContactInfo = newContactInfo;

        Raise(new LeadContactInfoChangedDomainEvent(
            Guid.NewGuid(),
            Id,
            newContactInfo with { },
            oldContactInfo));

        return this;
    }

    public Lead ChangeCost(decimal newCost)
    {
        var oldCost = Cost;
        Cost = newCost;

        Raise(new LeadChangedCostDomainEvent(
            Guid.NewGuid(),
            Id,
            newCost,
            oldCost));

        return this;
    }

    public Lead ChangeExpirationDateRange(DateTime newStartDate, DateTime newEndDate)
    {
        var oldStartDate = StartDate;
        var oldEndDate = EndDate;

        StartDate = newStartDate;
        EndDate = newEndDate;

        Raise(new LeadChangedExpirationDateRangeDomainEvent(
            Guid.NewGuid(),
            Id,
            newStartDate,
            newEndDate,
            oldStartDate,
            oldEndDate));

        return this;
    }

    public override object Clone()
    {
        var cloned = new Lead(Id, Name, ProductId, Cost, StartDate, EndDate)
        {
            ContactInfo = ContactInfo with { }
        };

        return cloned;
    }
}