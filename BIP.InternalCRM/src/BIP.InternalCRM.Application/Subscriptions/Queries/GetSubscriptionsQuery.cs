using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Subscriptions.QueryResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Subscriptions.Queries;

public record GetSubscriptionsQuery(
    PaginationOptions PaginationOptions,
    string? ProductName = null,
    string? CustomerName = null,
    string? InvoiceNumber = null
) : IRequest<IReadOnlyCollection<SubscriptionQueryResult>>
{
    public class Handler :
        IRequestHandler<GetSubscriptionsQuery, IReadOnlyCollection<SubscriptionQueryResult>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<SubscriptionQueryResult>> Handle(GetSubscriptionsQuery request,
            CancellationToken cancellationToken)
        {
            var subs = await _dbContext.Invoices
                .AsNoTracking()
                .Include(_ => _.Subscriptions)
                .UseIf(
                    request.InvoiceNumber.IsNotEmpty(),
                    query => query.Where(_ => _.Number == request.InvoiceNumber))
                .UseIf(
                    request.ProductName.IsNotEmpty(),
                    query => query
                        .Join(_dbContext.Products, i => i.ProductId, p => p.Id, (i, p) => new { i, p.Name })
                        .Where(j => j.Name == request.ProductName)
                        .Select(j => j.i))
                .UseIf(
                    request.CustomerName.IsNotEmpty(),
                    query => query
                        .Join(_dbContext.Customers, i => i.CustomerId, p => p.Id, (i, c) => new { i, c.Name })
                        .Where(j => j.Name == request.CustomerName)
                        .Select(j => j.i))
                .UsePagination(request.PaginationOptions)
                .ToListAsync(cancellationToken);

            return subs
                .SelectMany(i => i.Subscriptions
                    .Select(sub => new SubscriptionQueryResult(sub, i.Number)))
                .ToList();
        }
    }
}