using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Payments.QueryResult;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Payments.Queries;

public record GetPaymentsQuery(
    PaginationOptions PaginationOptions,
    string? PurchaseOrderNumber = null,
    string? InvoiceNumber = null,
    string? SubscriptionNumber = null,
    string? ProductName = null,
    string? CustomerName = null
) : IRequest<IReadOnlyCollection<PaymentQueryResult>>
{
    public class Handler :
        IRequestHandler<GetPaymentsQuery, IReadOnlyCollection<PaymentQueryResult>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<PaymentQueryResult>> Handle(
            GetPaymentsQuery request,
            CancellationToken cancellationToken)
        {
            var invoices = await _dbContext.Invoices
                .AsNoTracking()
                .UseIf(
                    request.InvoiceNumber.IsNotEmpty(),
                    q => q.Where(i => i.Number == request.InvoiceNumber))
                .UseIf(
                    request.PurchaseOrderNumber.IsNotEmpty(),
                    query => query
                        .Join(
                            _dbContext.PurchaseOrders,
                            res => res.PurchaseOrder!.Id,
                            po => po.Id,
                            (i, po) => new { i, po.Number })
                        .Where(res => res.Number == request.PurchaseOrderNumber)
                        .Select(res => res.i))
                .UseIf(
                    request.SubscriptionNumber.IsNotEmpty(),
                    query => query
                        .Include(_ => _.Subscriptions))
                .UseIf(
                    request.ProductName.IsNotEmpty(),
                    query => query
                        .Join(
                            _dbContext.Products,
                            res => res.ProductId,
                            p => p.Id,
                            (i, p) => new { i, p.Name })
                        .Where(res => res.Name == request.ProductName)
                        .Select(res => res.i))
                .UseIf(
                    request.CustomerName.IsNotEmpty(),
                    query => query
                        .Join(
                            _dbContext.Customers,
                            res => res.CustomerId,
                            c => c.Id,
                            (i, c) => new { i, c.Name })
                        .Where(res => res.Name == request.CustomerName)
                        .Select(res => res.i))
                .Include(_ => _.Payments)
                .ToListAsync(cancellationToken);

            var result = invoices
                .Condition(
                    request.SubscriptionNumber.IsNotEmpty(),
                    _ => _.Where(i => i.Subscriptions
                        .ContainsBy(sub => sub.Number
                            .Equals(request.SubscriptionNumber, StringComparison.InvariantCultureIgnoreCase))))
                .Select(_ => new { _.Payments, _.Number })
                .UsePagination(request.PaginationOptions)
                .SelectMany(_ => _.Payments
                    .Select(p => new PaymentQueryResult(p, _.Number)))
                .ToList();

            return result;
        }
    }
}