using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.PurchaseOrders.QueryResults;
using BIP.InternalCRM.Domain.PurchaseOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.PurchaseOrders.Queries;

public record GetPurchaseOrderQuery(
    string Number
) : IRequest<OneOf<PurchaseOrderQueryResult, NotFound>>
{
    public class Handler :
        IRequestHandler<GetPurchaseOrderQuery, OneOf<PurchaseOrderQueryResult, NotFound>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OneOf<PurchaseOrderQueryResult, NotFound>> Handle(
            GetPurchaseOrderQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.PurchaseOrders
                .AsNoTracking()
                .Where(_ => _.Number.Equals(request.Number))
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
                .FirstOrDefaultAsync(cancellationToken);

            return result is not null
                ? new PurchaseOrderQueryResult(result.po, result.ProductName, result.CustomerName)
                : new NotFound<PurchaseOrder>();
        }
    }
}
