using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Customers.Queries;

public record GetCustomersQuery(
    PaginationOptions PaginationOptions,
    string? ProductName = null
) : IRequest<IReadOnlyCollection<Customer>>
{
    public class Handler :
        IRequestHandler<GetCustomersQuery, IReadOnlyCollection<Customer>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IRelationsDbContext _relationsDbContext;

        public Handler(IDomainDbContext dbContext, IRelationsDbContext relationsDbContext)
        {
            _dbContext = dbContext;
            _relationsDbContext = relationsDbContext;
        }

        public async Task<IReadOnlyCollection<Customer>> Handle(GetCustomersQuery request,
            CancellationToken cancellationToken)
        {
            var customers = await _dbContext.Customers
                .AsNoTracking()
                .UseIf(
                    request.ProductName.IsNotEmpty(),
                    query => query
                        .Join(
                            _relationsDbContext.CustomersRelations,
                            c => c.Id,
                            cr => cr.CustomerId,
                            (c, cr) => new { c, cr })
                        .Join(
                            _dbContext.Products,
                            tpl => tpl.cr.ProductId,
                            p => p.Id,
                            (res, p) => new { res.c, p })
                        .Where(res => res.p.Name == request.ProductName)
                        .Select(res => res.c))
                .UsePagination(request.PaginationOptions)
                .ToListAsync(cancellationToken);

            return customers;
        }
    }
}