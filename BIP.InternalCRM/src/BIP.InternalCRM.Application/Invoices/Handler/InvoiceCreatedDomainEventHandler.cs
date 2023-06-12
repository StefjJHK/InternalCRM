using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Customers.Commands;
using BIP.InternalCRM.Domain.Invoices.DomainEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Invoices.Handler;

public class InvoiceCreatedDomainEventHandler
    : INotificationHandler<InvoiceCreatedDomainEvent>
{
    private readonly IDomainDbContext _domainDbContext;
    private readonly ISender _sender;

    public InvoiceCreatedDomainEventHandler(IDomainDbContext domainDbContext, ISender sender)
    {
        _domainDbContext = domainDbContext;
        _sender = sender;
    }

    public async Task Handle(InvoiceCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var sameInvoice = await _domainDbContext.Invoices
            .AsNoTracking()
            .Where(_ => _.Id != notification.InvoiceId)
            .Where(_ => _.CustomerId == notification.Invoice.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);

        var existPoByCustomer = await _domainDbContext.PurchaseOrders
            .AsNoTracking()
            .Where(_ => _.CustomerId == notification.Invoice.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);

        if (existPoByCustomer is not null || sameInvoice is not null)
        {
            return;
        }

        var addRelationCmd = new AddCustomerProductRelationCommand(
            notification.Invoice.CustomerId,
            notification.Invoice.ProductId);
        await _sender.Send(addRelationCmd, cancellationToken);
    }
}