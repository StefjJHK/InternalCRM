using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Leads.QueryResults;
using BIP.InternalCRM.Domain.Leads;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Leads.Queries;

public record GetLeadsQuery(
    PaginationOptions PaginationOptions,
    string? ProductName = null
) : IRequest<IReadOnlyCollection<LeadQueryResult>>
{
    public class Handler :
        IRequestHandler<GetLeadsQuery, IReadOnlyCollection<LeadQueryResult>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<LeadQueryResult>> Handle(GetLeadsQuery request,
            CancellationToken cancellationToken)
        {
            var leads = await _dbContext.Leads
                .AsNoTracking()
                .Join(_dbContext.Products, l => l.ProductId, p => p.Id, (l, p) => new { l, ProductName = p.Name })
                .UseIf(
                    request.ProductName.IsNotEmpty(),
                    query => query.Where(j => j.ProductName == request.ProductName))
                .UsePagination(request.PaginationOptions)
                .Select(j => new LeadQueryResult(j.l, j.ProductName))
                .ToListAsync(cancellationToken);

            return leads;
        }
    }
}