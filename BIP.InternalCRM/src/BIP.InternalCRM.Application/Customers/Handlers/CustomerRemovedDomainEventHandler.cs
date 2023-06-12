using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.Customers.DomainEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Customers.Handlers;

public class CustomerRemovedDomainEventHandler : INotificationHandler<CustomerRemovedDomainEvent>
{
    private readonly IDomainDbContext _dbContext;

    public CustomerRemovedDomainEventHandler(IDomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(CustomerRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var pos = await _dbContext.PurchaseOrders
            .Include(_ => _.Invoices)
            .ThenInclude(_ => _.Payments)
            .Include(_ => _.Invoices)
            .ThenInclude(_ => _.Subscriptions)
            .Where(_ => _.CustomerId == notification.CustomerId)
            .ToListAsync(cancellationToken);

        pos.ForEach(_ => _.Remove());

        var invoices = await _dbContext.Invoices
            .Include(_ => _.Payments)
            .Include(_ => _.Subscriptions)
            .Where(_ => _.CustomerId == notification.CustomerId)
            .Where(_ => _.PurchaseOrder == null)
            .ToListAsync(cancellationToken);

        invoices.ForEach(_ => _.Remove());
    }
}