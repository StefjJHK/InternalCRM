using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Invoices.DomainEvents;
using BIP.InternalCRM.Domain.Payments;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.PurchaseOrders;
using BIP.InternalCRM.Domain.Subscriptions;
using BIP.InternalCRM.Primitives.DomainDriven;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.Domain.Invoices;

public sealed class Invoice : Entity<InvoiceId>
{
    private Invoice(
        InvoiceId id,
        string number,
        decimal amount,
        DateTime receivedDate,
        DateTime dueDate,
        ProductId productId,
        CustomerId customerId)
        : base(id)
    {
        Number = number;
        Amount = amount;
        ReceivedDate = receivedDate;
        DueDate = dueDate;
        ProductId = productId;
        CustomerId = customerId;

        _payments = new List<Payment>();
        _subscriptions = new List<Subscription>();
    }

    public string Number { get; private set; }

    public decimal Amount { get; private set; }

    public DateTime ReceivedDate { get; private set; }

    public DateTime DueDate { get; private set; }

    public DateTime? PaidDate { get; private set; }

    public bool IsOverdue { get; private set; }

    public PurchaseOrder? PurchaseOrder { get; private set; }

    public ProductId ProductId { get; private set; }

    public CustomerId CustomerId { get; private set; }

    private readonly ICollection<Payment> _payments;
    public IReadOnlyCollection<Payment> Payments => _payments.ToList();

    public OneOf<Payment, ValueMustBeUnique<Payment>, DateMustBeInDateRange<Invoice>> AddPayment(
        PaymentId paymentId,
        string number,
        decimal amount,
        DateTime receivedDate)
    {
        var result = Payment.Create(
            Id,
            paymentId,
            number,
            _payments.Select(_ => _.Number),
            amount,
            receivedDate,
            receivedDate > DueDate);

        if (result.IsT1) return result.AsT1;

        _payments.Add(result.AsT0);

        Raise(new InvoiceAddedPaymentDomainEvent(
            Guid.NewGuid(),
            Id,
            result.AsT0));

        var calcResult = CalculatePaidDate();

        if (calcResult.Value is DomainError) return calcResult.AsT1;

        return result.AsT0;
    }

    public Payment ChangePaymentAmount(PaymentId paymentId, decimal newAmount)
    {
        var payment = _payments.First(_ => _.Id == paymentId);

        var result = payment.ChangeAmount(newAmount);

        CalculatePaidDate();

        return result;
    }

    public OneOf<Payment, ValueMustBeUnique<Payment>> ChangePaymentNumber(
        PaymentId paymentId,
        string newNumber)
    {
        var payment = _payments.First(_ => _.Id == paymentId);

        var changeResult = payment.ChangeNumber(newNumber, _payments.Select(_ => _.Number));

        return changeResult;
    }

    public OneOf<Payment, DateMustBeInDateRange<Invoice>> ChangePaymentReceivedDate(
        PaymentId paymentId,
        DateTime newReceivedDate)
    {
        var payment = _payments.First(_ => _.Id == paymentId);

        var result = payment.ChangeReceivedDate(newReceivedDate, newReceivedDate > DueDate);

        var calcResult = CalculatePaidDate();

        if (calcResult.Value is DomainError) return calcResult.AsT1;

        return result;
    }

    public Success RemovePayment(PaymentId paymentId)
    {
        var payment = _payments.First(_ => _.Id == paymentId);
        _payments.Remove(payment);
        payment.Remove();

        CalculatePaidDate();

        return new Success();
    }

    private readonly ICollection<Subscription> _subscriptions;
    public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.ToList();

    public OneOf<Subscription, ValueMustBeUnique<Subscription>, SubscriptionsCostsGreaterThanAmount> AddSubscription(
        SubscriptionId subscriptionId,
        string number,
        string subLegalEntity,
        decimal cost,
        DateTime validFrom,
        DateTime validUntil)
    {
        if (_subscriptions.Sum(_ => _.Cost) + cost > Amount)
        {
            return new SubscriptionsCostsGreaterThanAmount();
        }

        var sub = Subscription.Create(
            Id,
            subscriptionId,
            number,
            _subscriptions.Select(_ => _.Number),
            subLegalEntity,
            cost,
            validFrom,
            validUntil);

        if (sub.IsT1) return sub.AsT1;

        _subscriptions.Add(sub.AsT0);

        Raise(new InvoiceAddedSubscriptionDomainEvent(
            Guid.NewGuid(),
            Id,
            sub.AsT0));

        return sub.AsT0;
    }

    public static OneOf<Invoice, ValueMustBeUnique<Invoice>> Create(
        InvoiceId id,
        string number,
        IEnumerable<string> otherInvoicesNumbers,
        decimal amount,
        DateTime receivedDate,
        DateTime dueDate,
        ProductId productId,
        CustomerId customerId)
    {
        if (otherInvoicesNumbers.Contains(number, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<Invoice>(_ => _.Number);
        }

        var @new = new Invoice(
            id,
            number,
            amount,
            receivedDate,
            dueDate,
            productId,
            customerId);

        @new.Raise(new InvoiceCreatedDomainEvent(Guid.NewGuid(), @new.Id, @new));

        return @new;
    }

    public Success Remove()
    {
        var removedInvoice = (Clone() as Invoice)!;

        foreach (var payment in _payments)
        {
            payment.Remove();
        }

        foreach (var sub in _subscriptions)
        {
            sub.Remove();
        }

        Raise(new InvoiceRemovedDomainEvent(
            Guid.NewGuid(),
            Id,
            removedInvoice));

        return new Success();
    }

    public OneOf<Invoice, ValueMustBeUnique<Invoice>> ChangeNumber(
        string newNumber,
        IEnumerable<string> otherInvoicesNumbers)
    {
        if (otherInvoicesNumbers.Contains(newNumber, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<Invoice>(_ => _.Number);
        }

        var oldNumber = (Number.Clone() as string)!;
        Number = newNumber;

        Raise(new InvoiceChangedNumberDomainEvent(
            Guid.NewGuid(),
            Id,
            newNumber,
            oldNumber));

        return this;
    }

    public Invoice SetPurchaseOrder(PurchaseOrder purchaseOrder)
    {
        PurchaseOrder = purchaseOrder;
        ProductId = purchaseOrder.ProductId;
        CustomerId = purchaseOrder.CustomerId;

        Raise(new InvoiceSetPurchaseOrderDomainEvent(
            Guid.NewGuid(),
            Id,
            PurchaseOrder));

        return this;
    }

    public OneOf<Invoice, DateMustBeInDateRange<Invoice>> ChangeAmount(decimal newAmount)
    {
        var oldAmount = Amount;
        Amount = newAmount;

        Raise(new InvoiceChangedAmountDomainEvent(
            Guid.NewGuid(),
            Id,
            newAmount,
            oldAmount));

        var calcResult = CalculatePaidDate();

        if (calcResult.Value is DomainError) return calcResult.AsT1;

        return this;
    }

    public Invoice ChangeExpirationDateRange(DateTime newReceivedDate, DateTime newDueDate)
    {
        var oldReceivedDate = ReceivedDate;
        var oldDueDate = DueDate;
        ReceivedDate = newReceivedDate;
        DueDate = newDueDate;

        Raise(new InvoiceChangedExpirationDateRangeDomainEvent(
            Guid.NewGuid(),
            Id,
            newReceivedDate,
            newDueDate,
            oldReceivedDate,
            oldDueDate));

        foreach (var payment in _payments)
        {
            payment.IsOverdue = payment.ReceivedDate > DueDate;
        }

        return this;
    }

    private OneOf<Invoice, DateMustBeInDateRange<Invoice>> SetPaidDate(DateTime? paidDate)
    {
        var oldPaidDate = PaidDate;
        PaidDate = paidDate;

        Raise(new InvoiceSetPaidDateDomainEvent(
            Guid.NewGuid(),
            Id,
            paidDate,
            oldPaidDate));

        IsOverdue = paidDate > DueDate;
        PurchaseOrder?.SetPaidDate(paidDate);
        foreach (var sub in _subscriptions)
        {
            sub.SetPaidDate(paidDate);
        }
        
        return this;
    }

    private OneOf<Invoice, DateMustBeInDateRange<Invoice>> CalculatePaidDate()
    {
        if (_payments.Count == 0)
        {
            return this;
        }

        var paymentsAmount = _payments.Sum(_ => _.Amount);

        if (paymentsAmount < Amount)
        {
            SetPaidDate(null);
        }

        var lastPaidPayment = _payments.MaxBy(_ => _.ReceivedDate)!;
        var result = SetPaidDate(lastPaidPayment.ReceivedDate);

        if (result.Value is DomainError) return result.AsT1;

        IsOverdue = lastPaidPayment.IsOverdue;

        return this;
    }

    public override object Clone()
    {
        var cloned = new Invoice(Id, Number, Amount, ReceivedDate, DueDate, ProductId, CustomerId)
        {
            PurchaseOrder = PurchaseOrder
        };

        return cloned;
    }
}