using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Statistics.Products.Models;
using BIP.InternalCRM.Domain.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OneOf;

namespace BIP.InternalCRM.Application.Statistics.Products.Queries;

public record GetProductStatisticsQuery(
    PaginationOptions PaginationOptions,
    string? ProductName = null
) : IRequest<OneOf<ProductStatistics, NotFound, IReadOnlyCollection<ProductStatistics>>>
{
    public class Handler :
        IRequestHandler<GetProductStatisticsQuery,
            OneOf<ProductStatistics, NotFound, IReadOnlyCollection<ProductStatistics>>>
    {
        private readonly IDomainDbContext _domainDbContext;
        private readonly IRelationsDbContext _relationsDbContext;

        public Handler(IDomainDbContext domainDbContext, IRelationsDbContext relationsDbContext)
        {
            _domainDbContext = domainDbContext;
            _relationsDbContext = relationsDbContext;
        }

        public async Task<OneOf<ProductStatistics, NotFound, IReadOnlyCollection<ProductStatistics>>> Handle(
            GetProductStatisticsQuery request,
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
                    let totalCustomers = _relationsDbContext.CustomersRelations
                        .Count(_ => _.ProductId == product.Id)
                    group new { invoice, totalCustomers } by product into grp
                    select new ProductStatistics(grp.Key.Name)
                    {
                        TotalCustomers = grp.Select(_ => _.totalCustomers).Single(),
                        TotalSubscriptions = grp
                            .Select(_ => _.invoice)
                            .SelectMany(_ => _.Subscriptions)
                            .Count(),
                        TotalRevenue = grp
                            .Select(_ => _.invoice)
                            .SelectMany(_ => _.Payments)
                            .Sum(_ => _.Amount)
                    })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return request.ProductName.IsEmpty()
                ? result
                : result.Any()
                    ? result.First()
                    : new NotFound<ProductStatistics>();
        }
    }
}