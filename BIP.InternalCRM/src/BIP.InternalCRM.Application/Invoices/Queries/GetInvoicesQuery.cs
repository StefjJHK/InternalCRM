using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Invoices.QueryResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Invoices.Queries;

public record GetInvoicesQuery(
    PaginationOptions PaginationOptions,
    string? ProductName = null,
    string? CustomerName = null,
    string? PurchaseOrderNumber = null
) : IRequest<IReadOnlyCollection<InvoiceQueryResult>>
{
    public class Handler :
        IRequestHandler<GetInvoicesQuery, IReadOnlyCollection<InvoiceQueryResult>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<InvoiceQueryResult>> Handle(GetInvoicesQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.Invoices
                .AsNoTracking()
                .Include(_ => _.PurchaseOrder)
                .Join(
                    _dbContext.Products,
                    i => i.ProductId,
                    p => p.Id,
                    (i, p) => new { i, ProductName = p.Name })
                .Join(
                    _dbContext.Customers,
                    j => j.i.CustomerId,
                    p => p.Id,
                    (j, c) => new { j.i, j.ProductName, CustomerName = c.Name })
                .UseIf(
                    request.ProductName.IsNotEmpty(),
                    query => query.Where(j => j.ProductName == request.ProductName))
                .UseIf(
                    request.CustomerName.IsNotEmpty(),
                    query => query.Where(j => j.CustomerName == request.CustomerName))
                .UseIf(
                    request.PurchaseOrderNumber.IsNotEmpty(),
                    query => query
                        .Where(j => j.i.PurchaseOrder!.Number == request.PurchaseOrderNumber))
                .UsePagination(request.PaginationOptions)
                .ToListAsync(cancellationToken);

            return result
                .Select(j => new InvoiceQueryResult(j.i, j.ProductName, j.CustomerName))
                .ToList();
        }
    }
}