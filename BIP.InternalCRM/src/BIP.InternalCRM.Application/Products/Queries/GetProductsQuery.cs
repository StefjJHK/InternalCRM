using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Products.Queries;

public record GetProductsQuery(
    PaginationOptions PaginationOptions,
    string? CustomerName = null
) : IRequest<IReadOnlyCollection<Product>>
{
    public class Handler :
        IRequestHandler<GetProductsQuery, IReadOnlyCollection<Product>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IRelationsDbContext _relationsDbContext;

        public Handler(IDomainDbContext dbContext, IRelationsDbContext relationsDbContext)
        {
            _dbContext = dbContext;
            _relationsDbContext = relationsDbContext;
        }

        public async Task<IReadOnlyCollection<Product>> Handle(GetProductsQuery request,
            CancellationToken cancellationToken)
        {
            var products = await _dbContext.Products
                .AsNoTracking()
                .UseIf(
                    request.CustomerName.IsNotEmpty(),
                    query => query
                        .Join(
                            _relationsDbContext.CustomersRelations,
                            p => p.Id,
                            cr => cr.ProductId,
                            (p, cr) => new { Product = p, cr.CustomerId })
                        .Join(
                            _dbContext.Customers,
                            _ => _.CustomerId,
                            c => c.Id,
                            (_, c) => new { _.Product, _.CustomerId, CustomerName = c.Name })
                        .Where(_ => _.CustomerName == request.CustomerName)
                        .Select(_ => _.Product))
                .UsePagination(request.PaginationOptions)
                .ToListAsync(cancellationToken);

            return products;
        }
    }
}