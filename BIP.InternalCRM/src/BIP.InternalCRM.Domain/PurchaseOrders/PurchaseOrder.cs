using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.PurchaseOrders.DomainEvents;
using BIP.InternalCRM.Primitives.DomainDriven;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.Domain.PurchaseOrders;

public sealed class PurchaseOrder : Entity<PurchaseOrderId>
{
    private PurchaseOrder(
        PurchaseOrderId id,
        string number,
        decimal amount,
        DateTime receivedDate,
        DateTime dueDate,
        CustomerId customerId,
        ProductId productId)
        : base(id)
    {
        Number = number;
        Amount = amount;
        ReceivedDate = receivedDate;
        DueDate = dueDate;
        CustomerId = customerId;
        ProductId = productId;

        _invoices = new List<Invoice>();
    }

    public string Number { get; private set; }

    public decimal Amount { get; private set; }

    public DateTime ReceivedDate { get; private set; }

    public DateTime DueDate { get; private set; }

    public DateTime? PaidDate { get; private set; }

    public CustomerId CustomerId { get; }

    public ProductId ProductId { get; }

    private readonly ICollection<Invoice> _invoices;
    public IReadOnlyCollection<Invoice> Invoices => _invoices.ToList();

    public OneOf<Invoice, ValueMustBeUnique<Invoice>> AddInvoice(
        InvoiceId invoiceId,
        string number,
        decimal amount,
        DateTime receivedDate,
        DateTime dueDate)
    {
        // if (receivedDate < ReceivedDate || dueDate > DueDate)
        // {
        //     return new DateMustBeInDateRange<Invoi>()
        // }
        
        var createInvoiceResult = Invoice.Create(
            invoiceId,
            number,
            _invoices.Select(_ => _.Number),
            amount,
            receivedDate,
            dueDate,
            ProductId,
            CustomerId);

        if (createInvoiceResult.Value is DomainError) return createInvoiceResult.AsT1;
        
        var invoice = createInvoiceResult.AsT0.SetPurchaseOrder(this);

        _invoices.Add(invoice);
        
        Raise(new PurchaseOrderAddedInvoiceDomainEvent(
            Guid.NewGuid(),
            Id, 
            invoice));

        return invoice;
    }

    public static OneOf<PurchaseOrder, ValueMustBeUnique<PurchaseOrder>> Create(
        PurchaseOrderId id,
        string number,
        IEnumerable<string> otherPosNumbers,
        decimal amount,
        ProductId productId,
        CustomerId customerId,
        DateTime receivedDate,
        DateTime dueDate)
    {
        if (otherPosNumbers.Contains(number, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<PurchaseOrder>(_ => _.Number);
        }
        
        var @new = new PurchaseOrder(id, number, amount, receivedDate, dueDate, customerId, productId);

        @new.Raise(new PurchaseOrderCreatedDomainEvent(Guid.NewGuid(), @new.Id, @new));

        return @new;
    }

    public Success Remove()
    {
        var removedPo = (Clone() as PurchaseOrder)!;
        
        foreach (var invoice in _invoices)
        {
            invoice.Remove();
        }
        
        Raise(new PurchaseOrderRemovedDomainEvent(
            Guid.NewGuid(),
            Id,
            removedPo));

        return new Success();
    }

    public OneOf<PurchaseOrder, ValueMustBeUnique<PurchaseOrder>> ChangeNumber(
        string newNumber,
        IEnumerable<string> otherPosNumbers)
    {
        if (otherPosNumbers.Contains(newNumber, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<PurchaseOrder>(_ => _.Number);
        }

        var oldNumber = (Number.Clone() as string)!;
        Number = newNumber;
        
        Raise(new PurchaseOrderChangedNumberDomainEvent(
            Guid.NewGuid(),
            Id,
            newNumber,
            oldNumber));

        return this;
    }  

    public PurchaseOrder ChangeAmount(decimal newAmount)
    {
        var oldAmount = Amount;
        Amount = newAmount;

        Raise(new PurchaseOrderChangedAmountDomainEvent(
            Guid.NewGuid(),
            Id,
            newAmount,
            oldAmount));

        return this;
    }

    public PurchaseOrder ChangeExpirationDate(DateTime newReceivedDate, DateTime newDueDate)
    {
        var oldReceivedDate = ReceivedDate;
        var oldDueDate = DueDate;
        ReceivedDate = newReceivedDate;
        DueDate = newDueDate;
        
        Raise(new PurchaseOrderChangedExpirationDateDomainEvent(
            Guid.NewGuid(),
            Id,
            newReceivedDate,
            newDueDate,
            oldReceivedDate,
            oldDueDate));

        return this;
    }

    public PurchaseOrder SetPaidDate(DateTime? paidDate)
    {
        if (_invoices.Any(_ => !_.PaidDate.HasValue)) return this;
        
        var oldPaidDate = PaidDate;
        PaidDate = paidDate;
        
        Raise(new PurchaseOrderSetPaidDateDomainEvent(
            Guid.NewGuid(),
            Id,
            paidDate,
            oldPaidDate));

        return this;
    }

    public override object Clone()
    {
        var cloned = new PurchaseOrder(Id, Number, Amount, ReceivedDate, DueDate, CustomerId, ProductId);

        return cloned;
    }
}
