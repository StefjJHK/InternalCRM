using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.Products.DomainEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Products.Handlers;

public class ProductRemovedDomainEventHandler : INotificationHandler<ProductRemovedDomainEvent>
{
    private readonly IDomainDbContext _dbContext;

    public ProductRemovedDomainEventHandler(IDomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ProductRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        var leads = await _dbContext.Leads
            .Where(_ => _.ProductId == notification.ProductId)
            .ToListAsync(cancellationToken);

        leads.ForEach(_ => _.Remove());

        var pos = await _dbContext.PurchaseOrders
            .Include(_ => _.Invoices)
            .ThenInclude(_ => _.Payments)
            .Include(_ => _.Invoices)
            .ThenInclude(_ => _.Subscriptions)
            .Where(_ => _.ProductId == notification.ProductId)
            .ToListAsync(cancellationToken);

        pos.ForEach(_ => _.Remove());

        var invoices = await _dbContext.Invoices
            .Include(_ => _.Payments)
            .Include(_ => _.Subscriptions)
            .Where(_ => _.ProductId == notification.ProductId)
            .Where(_ => _.PurchaseOrder == null)
            .ToListAsync(cancellationToken);

        invoices.ForEach(_ => _.Remove());
    }
}