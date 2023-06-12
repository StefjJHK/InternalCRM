using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Customers.Commands;
using BIP.InternalCRM.Domain.PurchaseOrders.DomainEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.PurchaseOrders.Handlers;

public class PurchaseOrderCreatedDomainEventHandler :
    INotificationHandler<PurchaseOrderCreatedDomainEvent>
{
    private readonly IDomainDbContext _domainDbContext;
    private readonly ISender _sender;

    public PurchaseOrderCreatedDomainEventHandler(IDomainDbContext domainDbContext, ISender sender)
    {
        _domainDbContext = domainDbContext;
        _sender = sender;
    }

    public async Task Handle(PurchaseOrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var samePo = await _domainDbContext.PurchaseOrders
            .AsNoTracking()
            .Where(_ => _.Id != notification.PurchaseOrderId)
            .Where(_ => _.CustomerId == notification.PurchaseOrder.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);

        var invoice = await _domainDbContext.Invoices
            .AsNoTracking()
            .Where(_ => _.CustomerId == notification.PurchaseOrder.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);

        if (invoice is not null || samePo is not null)
        {
            return;
        }

        var addRelationCmd = new AddCustomerProductRelationCommand(
            notification.PurchaseOrder.CustomerId,
            notification.PurchaseOrder.ProductId);
        await _sender.Send(addRelationCmd, cancellationToken);
    }
}