using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Payments.DomainEvents;
using BIP.InternalCRM.Primitives.DomainDriven;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.Domain.Payments;

public sealed class Payment : Entity<PaymentId>
{
    private Payment(
        InvoiceId invoiceId,
        PaymentId id,
        string number,
        decimal amount,
        DateTime receivedDate,
        bool isOverdue
    ) : base(id)
    {
        InvoiceId = invoiceId;
        Number = number;
        Amount = amount;
        ReceivedDate = receivedDate;
        IsOverdue = isOverdue;
    }

    public string Number { get; private set; }

    public InvoiceId InvoiceId { get; }

    public decimal Amount { get; private set; }

    public DateTime ReceivedDate { get; private set; }

    public bool IsOverdue { get; set; }

    public static OneOf<Payment, ValueMustBeUnique<Payment>> Create(
        InvoiceId invoiceId,
        PaymentId id,
        string number,
        IEnumerable<string> otherPaymentsNumbers,
        decimal amount,
        DateTime receivedDate,
        bool isExpired)
    {
        if (otherPaymentsNumbers.Contains(number, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<Payment>(_ => _.Number);
        }

        var @new = new Payment(invoiceId, id, number, amount, receivedDate, isExpired);

        @new.Raise(new PaymentCreatedDomainEvent(Guid.NewGuid(), @new.Id, @new));

        return @new;
    }

    public Success Remove()
    {
        var removedPayment = (Clone() as Payment)!;

        Raise(new PaymentRemovedDomainEvent(Guid.NewGuid(), Id, removedPayment));

        return new Success();
    }

    public OneOf<Payment, ValueMustBeUnique<Payment>> ChangeNumber(
        string newNumber,
        IEnumerable<string> otherPaymentsNumbers)
    {
        if (otherPaymentsNumbers.Contains(newNumber, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<Payment>(_ => _.Number);
        }

        var oldNumber = (Number.Clone() as string)!;
        Number = newNumber;

        Raise(new PaymentChangedNumberDomainEvent(
            Guid.NewGuid(),
            Id,
            newNumber,
            oldNumber));

        return this;
    }

    public Payment ChangeAmount(decimal newAmount)
    {
        var oldAmount = Amount;
        Amount = newAmount;

        Raise(new PaymentChangedAmountDomainEvent(
            Guid.NewGuid(),
            Id,
            newAmount,
            oldAmount));

        return this;
    }

    public Payment ChangeReceivedDate(DateTime newReceivedDate, bool newIsOverdue)
    {
        var oldReceivedDate = ReceivedDate;
        var oldIsOverdue = IsOverdue;
        ReceivedDate = newReceivedDate;
        IsOverdue = newIsOverdue;

        Raise(new PaymentChangedReceivedDateDomainEvent(
            Guid.NewGuid(),
            Id,
            newReceivedDate,
            oldReceivedDate,
            newIsOverdue,
            oldIsOverdue));

        return this;
    }

    public override object Clone()
    {
        var cloned = new Payment(InvoiceId, Id, Number, Amount, ReceivedDate, IsOverdue);

        return cloned;
    }
}