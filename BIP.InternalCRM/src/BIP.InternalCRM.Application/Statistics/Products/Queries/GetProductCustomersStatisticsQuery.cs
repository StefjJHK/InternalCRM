using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Statistics.Products.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Statistics.Products.Queries;

public record GetProductCustomersStatisticsQuery(
    PaginationOptions PaginationOptions,
    string? ProductName = null
) : IRequest<IReadOnlyCollection<ProductCustomersStatistics>>
{
    public class Handler :
        IRequestHandler<GetProductCustomersStatisticsQuery, IReadOnlyCollection<ProductCustomersStatistics>>
    {
        private readonly IDomainDbContext _domainDbContext;
        private readonly IRelationsDbContext _relationsDbContext;

        public Handler(IDomainDbContext domainDbContext, IRelationsDbContext relationsDbContext)
        {
            _domainDbContext = domainDbContext;
            _relationsDbContext = relationsDbContext;
        }


        public async Task<IReadOnlyCollection<ProductCustomersStatistics>> Handle(
            GetProductCustomersStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var result = await (
                from product in _domainDbContext.Products
                    .UseIf(
                        request.ProductName.IsNotEmpty(),
                        q => q.Where(_ => _.Name == request.ProductName))
                from invoice in _domainDbContext.Invoices
                    .Where(_ => _.ProductId == product.Id)
                    .DefaultIfEmpty()
                let totalPos =  _domainDbContext.PurchaseOrders
                    .Count(_ => _.ProductId == product.Id)
                join productCustomerRelation in _relationsDbContext.CustomersRelations
                    on product.Id equals productCustomerRelation.ProductId
                    into productCustomerRelations
                from productCustomerRelation in productCustomerRelations.DefaultIfEmpty()
                join customer in _domainDbContext.Customers
                    on productCustomerRelation.CustomerId equals customer.Id
                    into productCustomers
                from productCustomer in productCustomers
                group new { product, invoice, totalPos } by productCustomer into grp
                select new ProductCustomersStatistics(
                    grp.Select(_ => _.product.Name).Single(),
                    grp.Key.Name)
                {
                    TotalPurchaseOrders = grp.Select(_ => _.totalPos).First(),
                    TotalInvoices = grp.Select(_ => _.invoice).Count(),
                    TotalSubscriptions = grp.Select(_ => _.invoice).SelectMany(_ => _.Subscriptions).Count(),
                    TotalRevenue =grp.Select(_ => _.invoice).SelectMany(_ => _.Payments).Sum(_ => _.Amount)
                })
            .AsNoTracking()
            .AsSplitQuery()
            .UsePagination(request.PaginationOptions)
            .ToListAsync(cancellationToken);

            return result;
        }
    }
}