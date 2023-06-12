using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Leads.QueryResults;
using BIP.InternalCRM.Domain.Leads;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Leads.Queries;

public record GetLeadQuery(
    string Name
) : IRequest<OneOf<LeadQueryResult, NotFound>>
{
    public class Handler :
        IRequestHandler<GetLeadQuery, OneOf<LeadQueryResult, NotFound>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OneOf<LeadQueryResult, NotFound>> Handle(GetLeadQuery request,
            CancellationToken cancellationToken)
        {
            var lead = await (
                    from l in _dbContext.Leads
                    where l.Name == request.Name
                    join p in _dbContext.Products
                        on l.ProductId equals p.Id
                    select new LeadQueryResult(l, p.Name))
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            return lead is not null
                ? lead
                : new NotFound<Lead>();
        }
    }
}