using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.Subscriptions.DomainEvents;
using BIP.InternalCRM.Primitives.DomainDriven;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.Domain.Subscriptions;

public class Subscription : Entity<SubscriptionId>
{
    private Subscription(
        InvoiceId invoiceId,
        SubscriptionId id,
        string number,
        string subLegalEntity,
        decimal cost,
        DateTime validFrom,
        DateTime validUntil
    )
        : base(id)
    {
        InvoiceId = invoiceId;
        Number = number;
        SubLegalEntity = subLegalEntity;
        Cost = cost;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
    }

    public string Number { get; }

    public InvoiceId InvoiceId { get; }

    public string SubLegalEntity { get; private set; }

    public decimal Cost { get; private set; }

    public DateTime? PaidDate { get; private set; }

    public DateTime ValidFrom { get; private set; }

    public DateTime ValidUntil { get; private set; }

    public IntelliLockLicense? License { get; private set; }

    public static OneOf<Subscription, ValueMustBeUnique<Subscription>> Create(
        InvoiceId invoiceId,
        SubscriptionId id,
        string number,
        IEnumerable<string> otherSubsNumbers,
        string subLegalEntity,
        decimal cost,
        DateTime validFrom,
        DateTime validUntil)
    {
        if (otherSubsNumbers.Contains(number, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<Subscription>(_ => _.Number);
        }

        var @new = new Subscription(invoiceId, id, number, subLegalEntity, cost, validFrom, validUntil);

        @new.Raise(new SubscriptionCreatedDomainEvent(Guid.NewGuid(), @new.Id, @new));

        return @new;
    }

    public Success Remove()
    {
        var removedSub = (Clone() as Subscription)!;

        RemoveIlLicense(null, true);
        
        Raise(new SubscriptionRemovedDomainEvent(
            Guid.NewGuid(),
            Id,
            removedSub));

        return new Success();
    }

    public Subscription AddIlLicense(Guid key, byte[] licData)
    {
        var licFilename = $"{SubLegalEntity}_{Guid.NewGuid()}.licence";
        var newLicense = IntelliLockLicense.Create(key, licFilename, licData);

        if (License == null)
        {
            License = newLicense;
            
            Raise(new SubscriptionAddedIlLicenseDomainEvent(
                Guid.NewGuid(),
                Id,
                this,
                License));
        }
        else
        {
            var oldLicense = License with { };
            License = newLicense;
            
            Raise(new SubscriptionChangedIlLicenseDomainEvent(
                Guid.NewGuid(),
                Id,
                newLicense,
                oldLicense));
        }

        return this;
    }

    public OneOf<Success, IlLicenseCannotBeRemovedWithExistingIlProject> RemoveIlLicense(
        IntelliLockProject? ilProject,
        bool isSubRemove = false)
    {
        if (ilProject == null && !isSubRemove)
        {
            return new IlLicenseCannotBeRemovedWithExistingIlProject(); 
        }

        if (License is null)
        {
            return new Success();
        }

        var removedLicense = License with { };
        License = null;
        
        Raise(new SubscriptionRemovedIlLicenseDomainEvent(
            Guid.NewGuid(),
            Id,
            removedLicense));

        return new Success();
    }

    public Subscription ChangeSubLegalEntity(string newSubLegalEntity)
    {
        var oldSubLegalEntity = SubLegalEntity;
        SubLegalEntity = newSubLegalEntity;

        Raise(new SubscriptionChangedSubLegalEntityDomainEvent(
            Guid.NewGuid(),
            Id,
            newSubLegalEntity,
            oldSubLegalEntity));

        return this;
    }

    public Subscription ChangeCost(decimal newCost)
    {
        var oldCost = Cost;
        Cost = newCost;

        Raise(new SubscriptionChangedCostDomainEvent(
            Guid.NewGuid(),
            Id,
            newCost,
            oldCost));

        return this;
    }

    public Subscription ChangeValidDateRange(DateTime newValidFrom, DateTime newValidUntil)
    {
        var oldValidFrom = ValidFrom;
        var oldValidUntil = ValidUntil;
        ValidFrom = newValidFrom;
        ValidUntil = newValidUntil;

        Raise(new SubscriptionChangedValidDateRangeDomainEvent(
            Guid.NewGuid(),
            Id,
            newValidFrom,
            newValidUntil,
            oldValidFrom,
            oldValidUntil));

        return this;
    }

    public Subscription SetPaidDate(DateTime? paidDate)
    {
        var oldPaidDate = PaidDate;
        PaidDate = paidDate;

        Raise(new SubscriptionSetPaidDateDomainEvent(
            Guid.NewGuid(),
            Id,
            paidDate,
            oldPaidDate));

        return this;
    }

    public override object Clone()
    {
        var cloned = new Subscription(InvoiceId, Id, Number, SubLegalEntity, Cost, ValidFrom, ValidUntil);

        if (License != null)
        {
            cloned.License = License with { };
        }

        return cloned;
    }
}