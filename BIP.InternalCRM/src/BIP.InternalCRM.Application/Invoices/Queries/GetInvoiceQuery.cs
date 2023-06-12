using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Invoices.QueryResults;
using BIP.InternalCRM.Domain.Invoices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Invoices.Queries;

public record GetInvoiceQuery(
    string Number
) : IRequest<OneOf<InvoiceQueryResult, NotFound<Invoice>>>
{
    public class Handler :
        IRequestHandler<GetInvoiceQuery, OneOf<InvoiceQueryResult, NotFound<Invoice>>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OneOf<InvoiceQueryResult, NotFound<Invoice>>> Handle(
            GetInvoiceQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.Invoices
                .AsNoTracking()
                .Where(_ => _.Number == request.Number)
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
                .FirstOrDefaultAsync(cancellationToken);

            return result is not null
                ? new InvoiceQueryResult(result.i, result.ProductName, result.CustomerName)
                : new NotFound<Invoice>();
        }
    }
}
