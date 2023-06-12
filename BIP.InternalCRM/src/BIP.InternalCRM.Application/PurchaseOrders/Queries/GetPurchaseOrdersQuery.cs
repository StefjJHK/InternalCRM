using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.PurchaseOrders.QueryResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.PurchaseOrders.Queries;

public record GetPurchaseOrdersQuery(
    PaginationOptions PaginationOptions,
    string? InvoiceNumber = null,
    string? ProductName = null,
    string? CustomerName = null
) : IRequest<IReadOnlyCollection<PurchaseOrderQueryResult>>
{


    public class Handler :
        IRequestHandler<GetPurchaseOrdersQuery, IReadOnlyCollection<PurchaseOrderQueryResult>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<PurchaseOrderQueryResult>> Handle(
            GetPurchaseOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.PurchaseOrders
                .AsNoTracking()
                .UseIf(
                    request.InvoiceNumber.IsNotEmpty(),
                    query => query
                        .Include(po => po.Invoices
                            .Where(_ => _.Number == request.InvoiceNumber)))
                .Join(
                    _dbContext.Products,
                    po => po.ProductId,
                    p => p.Id,
                    (po, p) => new { po, ProductName = p.Name })
                .Join(
                    _dbContext.Customers,
                    j => j.po.CustomerId,
                    c => c.Id,
                    (j, c) => new { j.po, j.ProductName, CustomerName = c.Name })
                .UseIf(
                    request.ProductName.IsNotEmpty(),
                    query => query.Where(j => j.ProductName == request.ProductName))
                .UseIf(
                    request.CustomerName.IsNotEmpty(),
                    query => query.Where(j => j.CustomerName == request.CustomerName))
                .UsePagination(request.PaginationOptions)
                .ToListAsync(cancellationToken);

            return result
                .ConditionalWhere(
                    request.InvoiceNumber.IsNotEmpty(),
                    _ => _.po.Invoices.Any())
                .Select(j => new PurchaseOrderQueryResult(j.po, j.CustomerName, j.ProductName))
                .ToList();
        }
    }
}